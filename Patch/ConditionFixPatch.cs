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
    class ConditionFixPatch
    {
        [HarmonyPatch(typeof(Chara), nameof(Chara.Tick)), HarmonyPostfix]
        public static void HandleTickPostfixActionPatch(Chara __instance)
        {
            if (__instance.currentZone is ISpecialNefia)
            {
                (__instance.currentZone as ISpecialNefia).InvokeTickPostfixActions(__instance);
            }
        }
    }
}
