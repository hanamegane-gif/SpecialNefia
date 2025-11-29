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
    class InheritSpecialTypePatch
    {
        [HarmonyPatch(typeof(TraitNewZone), nameof(TraitNewZone.CreateZone)), HarmonyPostfix]
        public static void InheritPatch(Zone __result)
        {
            if (__result is ISpecialNefia)
            {
                (__result as ISpecialNefia).Init();
            }
        }
    }
}
