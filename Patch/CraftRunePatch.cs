using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.Patch
{
    [HarmonyPatch]
    class CraftRunePatch
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(RecipeCard), nameof(RecipeCard.Craft))]
        internal static IEnumerable<CodeInstruction> CraftExchangePatch(IEnumerable<CodeInstruction> instructions)
        {
            var MP = AccessTools.Method(typeof(RecipeCard), nameof(RecipeCard.Craft)).GetParameters();
            var argPosition = 114514;

            foreach (var pi in MP)
            {
                if (pi.Name == "model")
                {
                    argPosition = pi.Position + 1;
                    break;
                }
            }

            var cm = new CodeMatcher(instructions)
                .MatchEndForward(
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(RecipeCard), "get_idCard")),
                    new CodeMatch(OpCodes.Stloc_0)
                //IL_0000: ldarg.0
                //IL_0001: call instance string RecipeCard::get_idCard()
                //IL_0006: stloc.0
                )
                .Advance(1)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(RecipeCard), "get_idCard")),
                    new CodeInstruction(OpCodes.Ldarg_S, argPosition),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CraftRunePatch), nameof(ExchangeCraftKey))),
                    new CodeInstruction(OpCodes.Stloc_0)
                )
                .MatchEndForward(
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Trait), nameof(Trait.OnCrafted)))
                //IL_0406: callvirt instance void Trait::OnCrafted(class Recipe)
                )
                .Advance(1)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldarg_3),//ing
                    new CodeInstruction(OpCodes.Ldloc_S, 10),//thing
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(CraftRunePatch), nameof(SetItemAttributes)))
                )
                .InstructionEnumeration();

            return cm;
        }

        internal static string ExchangeCraftKey(string idCard, bool isCraftModel)
        {
            if (!isCraftModel && idCard == "MOD_byakko_SPN_rune_free")
            {
                return "rene_free";
            }

            return idCard;
        }

        internal static void SetItemAttributes(List<Thing> ingridients, Thing craftedItem)
        {
            var rune = ingridients.Find(i => i.id == "rune");

            if (rune != null)
            {
                craftedItem.refVal = rune.refVal;
                craftedItem.encLV = rune.encLV;
            }
            else
            {
                craftedItem.encLV = 0;
            }
        }
    }
}
