using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SpecialNefia.Deploy
{
    class DeployTypeFallback
    {
        internal static void DeployTypeFallbackSetting()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var targetNamespaceList = new List<string> 
            {
                null, //ZoneSpecialNefia
                "SpecialNefia.SpecialZoneEvent",
            };
            var typeFallbackSetting = ReadTypeFallbackSetting();
            bool shouldFileUpdate = false;

            foreach (var targetNamespace in targetNamespaceList)
            {
                var classes = assembly.GetTypes().Where(t => t.IsClass && t.Namespace == targetNamespace);


                foreach (var type in classes)
                {
                    string className = type.Name;
                    string baseClass = "";

                    if (typeof(Zone_RandomDungeon).IsAssignableFrom(type))
                    {
                        baseClass = "Zone_RandomDungeon";
                    }

                    if (typeof(ZoneEvent).IsAssignableFrom(type))
                    {
                        baseClass = "ZoneEvent";
                    }

                    if (baseClass == "")
                    {
                        continue;
                    }

                    string fallbackRowString = assembly.GetName().Name + "," + (targetNamespace + ((targetNamespace.IsEmpty()) ? "" : ".") + className) + "," + baseClass;

                    if (!typeFallbackSetting.Contains(fallbackRowString))
                    {
                        typeFallbackSetting.Add(fallbackRowString);
                        shouldFileUpdate = true;
                    }
                }
            }


            if (shouldFileUpdate)
            {
                string path = Path.Combine(CorePath.RootData, "type_resolver.txt");
                IO.SaveTextArray(path, typeFallbackSetting.ToArray());
            }
        }

        private static List<string> ReadTypeFallbackSetting()
        {
            string text = "type_resolver.txt";
            string[] array = new string[0];
            if (File.Exists(CorePath.RootData + text))
            {
                array = IO.LoadTextArray(CorePath.RootData + text);
            }
            else
            {
                array = new string[2] { "TrueArena,ArenaWaveEvent,ZoneEvent", "Elin-GeneRecombinator,Elin_GeneRecombinator.IncubationSacrifice,Chara" };
                IO.SaveTextArray(CorePath.RootData + text, array);
            }

            return array.ToList();
        }
    }
}
