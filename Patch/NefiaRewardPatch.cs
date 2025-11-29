using HarmonyLib;
using SpecialNefia.Nefia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.Patch
{
    [HarmonyPatch]
    class NefiaRewardPatch
    {
        [HarmonyPatch(typeof(Chara), nameof(Chara.TryDropBossLoot)), HarmonyPrefix]
        public static bool InheritPatch(Chara __instance)
        {
            if (EClass._zone is ISpecialNefia && EClass._zone.Boss == __instance)
            {
                (EClass._zone as ISpecialNefia).SpawnRewardChests(__instance);
            }

            return true;
        }
    }
}
