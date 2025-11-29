using HarmonyLib;
using SpecialNefia.Nefia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpecialNefia.Patch
{
    [HarmonyPatch]
    class SpeedPatch
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Chara), nameof(Chara.RefreshSpeed))]
        internal static IEnumerable<CodeInstruction> RefreshSpeedPatch(IEnumerable<CodeInstruction> instructions)
        {
            var ci = new CodeMatcher(instructions)
                .MatchStartForward
                (
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldfld),
                    new CodeMatch(OpCodes.Ldloc_0)

                    //IL_064e: ldarg.0
                    //IL_064f: ldarg.0
                    //IL_0650: ldfld int64 Chara::_Speed
                )
                .Advance(1)
                .InsertAndAdvance
                (
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(SpeedPatch), nameof(FixSpeed))),
                    new CodeInstruction(OpCodes.Stloc_0)
                )
                .InstructionEnumeration();
            return ci;
        }

        // 速度に作用するルールでは倍率をかけて調整する
        internal static int FixSpeed(Chara c, int speedMul, Element.BonusInfo info)
        {
            if (!(c.currentZone is ISpecialNefia))
            {
                return speedMul;
            }

            return (c.currentZone as ISpecialNefia).GetSpeedFix(c, speedMul, info);
        }
    }
}
