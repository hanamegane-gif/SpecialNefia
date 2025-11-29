using HarmonyLib;
using SpecialNefia.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.Patch
{
    [HarmonyPatch]
    class DisableCraftFeaturePatch
    {
        [HarmonyPatch(typeof(Scene), nameof(Scene.Init)), HarmonyPostfix]
        internal static void CraftRecipePatch(Scene.Mode newMode)
        {
            if (ModConfig.EnableLawlessRuneFeature)
            {
                return;
            }

            if (newMode == Scene.Mode.StartGame)
            {
                var modLawlessRune = EClass.sources.cards.rows.Find(card => card.id == "MOD_byakko_SPN_rune_free");
                var index = EClass.sources.cards.rows.IndexOf(modLawlessRune);

                EClass.sources.cards.rows[index].factory = new string[] { "" };
            }
        }
    }
}
