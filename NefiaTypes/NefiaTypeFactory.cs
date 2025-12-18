using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class NefiaTypeFactory
    {
        internal static List<NefiaType> CreateRandomNefiaTypes(Zone_RandomDungeon nefia, int typeNum)
        {
            return Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(NefiaType)))
                                     .Select(i => Activator.CreateInstance(i))
                                     .Cast<NefiaType>()
                                     .Where(t => t.IsMeetRequirement(nefia))
                                     .OrderBy(_ => EClass.rnd(114514)).Take(typeNum)
                                     .ToList();
        }
    }
}
