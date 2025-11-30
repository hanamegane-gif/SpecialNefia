using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using static TextureReplace;
using UnityEngine;
using static LayerFaith;
using SpecialNefia.Config;
using SpecialNefia.Nefia;
using SpecialNefia.NefiaTypes;

namespace SpecialNefia.Patch
{
    [HarmonyPatch]
    internal class GenerateSpecialNefiaPatch
    {
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Region), nameof(Region.CreateRandomSite), new Type[] { typeof(Point), typeof(string), typeof(bool), typeof(int) })]
        internal static IEnumerable<CodeInstruction> ConvertSpecialNefiaPatch(IEnumerable<CodeInstruction> instructions)
        {
            var MP = AccessTools.Method(typeof(Region), nameof(Region.CreateRandomSite), new Type[] { typeof(Point), typeof(string), typeof(bool), typeof(int) }).GetParameters();
            var argPosition = 114514;

            foreach (var pi in MP)
            {
                if (pi.Name == "lv")
                {
                    argPosition = pi.Position + 1;
                    break;
                }
            }

            var ci = new CodeMatcher(instructions)
                .MatchStartForward(
                    new CodeMatch(OpCodes.Ldarg_2),
                    new CodeMatch(OpCodes.Ldarg_0),
                    new CodeMatch(OpCodes.Ldc_I4_1)

                    //IL_001a: ldarg.2
                    //IL_001b: ldarg.0
                    //IL_001c: ldc.i4.1
                )
                .InsertAndAdvance(
                    new CodeMatch(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GenerateSpecialNefiaPatch), nameof(ConvertToSpecialNefia))),
                    new CodeInstruction(OpCodes.Starg_S, 2)
                )
                .MatchEndForward(
                    new CodeMatch(OpCodes.Ldloc_0),
                    new CodeMatch(OpCodes.Ldc_I4_1),
                    new CodeMatch(OpCodes.Callvirt, AccessTools.Method(typeof(Spatial), "set_isRandomSite"))

                    //IL_0103: ldloc.0
                    //IL_0104: ldc.i4.1
                    //IL_0105: callvirt instance void Spatial::set_isRandomSite(bool)
                )
                .Advance(1)
                .InsertAndAdvance(
                    new CodeInstruction(OpCodes.Ldloc_0),
                    new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(GenerateSpecialNefiaPatch), nameof(InitSpecialType)))
                )
                .InstructionEnumeration();

            return ci;
        }

        internal static string ConvertToSpecialNefia(string idZone)
        {
            return (ModConfig.SpecialNefiaChance > EClass.rnd(100)) ? "byakko_mod_spn_" + idZone : idZone;
        }

        internal static void InitSpecialType(Zone generatedNefia)
        {
            if (generatedNefia is ISpecialNefia)
            {
                (generatedNefia as ISpecialNefia).Init();
            }
        }
    }
}
