using HarmonyLib;
using SpecialNefia.Nefia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.Patch
{
    [HarmonyPatch]
    class DamageFixPatch
    {
        [HarmonyTargetMethod]
        public static MethodBase TargetMethod()
        {
            var method = typeof(Card).GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                      .Where(m => m.Name == "DamageHP" && m.GetParameters().Length > 4)
                                      .FirstOrDefault();

            return method;
        }

        [HarmonyPrefix]
        public static bool HandleDamageHPPrefixAction(Card __instance, ref long dmg, int ele, ref AttackSource attackSource, Card origin)
        {
            if (EClass._zone is ISpecialNefia)
            {
                (EClass._zone as ISpecialNefia).InvokeDamageFixAction(__instance, ref dmg, ele, ref attackSource);
                (EClass._zone as ISpecialNefia).InvokeDamageHPPrefixActions(__instance, origin);
            }

            return true;
        }

        [HarmonyPostfix]
        public static void HandleDamageHPPostfixAction(Card __instance, Card origin)
        {
            if (EClass._zone is ISpecialNefia)
            {
                (EClass._zone as ISpecialNefia).InvokeDamageHPPostfixActions(__instance, origin);
            }

            return;
        }
    }
}
