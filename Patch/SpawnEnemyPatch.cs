using HarmonyLib;
using SpecialNefia.Nefia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static BiomeProfile;
using static LayerFaith;

namespace SpecialNefia.Patch
{
    [HarmonyPatch]
    class SpawnEnemyPatch
    {
        [HarmonyPatch(typeof(Zone), nameof(Zone.SpawnMob)), HarmonyPostfix]
        internal static void HandleSpawnsPostfixActionPatch(Chara __result)
        {
            if (EClass._zone is ISpecialNefia)
            {
                (EClass._zone as ISpecialNefia).InvokeSpawnsPostfixActions(__result);
            }
            return;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Zone), nameof(Zone.SpawnMob))]
        internal static IEnumerable<CodeInstruction> SpawnMobPatch(IEnumerable<CodeInstruction> instructions)
        {
            var ci = new CodeMatcher(instructions)
                .MatchEndForward
                (
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(SpawnList), nameof(SpawnList.Select))),
                    new CodeMatch(ci =>
                        (ci.opcode == OpCodes.Stloc_S || ci.opcode == OpCodes.Stloc) &&
                        (ci.operand is byte b && b == 5 || ci.operand is LocalBuilder lb && lb.LocalIndex == 5))
                )
                .Advance(1)
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc_S, 5),
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SpawnEnemyPatch), nameof(HandleSpawnListFixActionPatch))),
                    new CodeInstruction(OpCodes.Stloc_S, 5)
                )
                .MatchEndForward
                (
                    new CodeMatch(OpCodes.Ldloc_3),
                    new CodeMatch(OpCodes.Call, AccessTools.Method(typeof(CardBlueprint), nameof(CardBlueprint.Set)))

                    //IL_0496: ldloc.3
                    //IL_0497: call void CardBlueprint::Set(class CardBlueprint)
                )
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc_S, 5),
                    new CodeInstruction(OpCodes.Ldloc_3),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SpawnEnemyPatch), nameof(HandleEnemyStrengthFixActionPatch)))
                )
                .InstructionEnumeration();
            return ci;
        }

        public static CardRow HandleSpawnListFixActionPatch(Zone currentZone, CardRow original, SpawnSetting spawnSetting)
        {
            if (currentZone is ISpecialNefia)
            {
                return (currentZone as ISpecialNefia).InvokeSpawnListFixAction(original, spawnSetting);
            }
            else
            {
                return original;
            }
        }

        public static void HandleEnemyStrengthFixActionPatch(Zone currentZone, CardRow original, CardBlueprint blueprint)
        {
            if (currentZone is ISpecialNefia)
            {
                (currentZone as ISpecialNefia).InvokeEnemyStrengthFixActions(original, blueprint);
            }
        }
    }
}
