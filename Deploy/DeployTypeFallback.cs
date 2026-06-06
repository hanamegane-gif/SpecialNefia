using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SpecialNefia.Deploy
{
    class DeployTypeFallback
    {
        private static List<string> TargetNamespaceList { get; } = new List<string>
        {
            null, //ZoneSpecialNefia
            "SpecialNefia.SpecialZoneEvent",
        };

        private static Dictionary<Type, string> BaseClassResolver { get; } = new Dictionary<Type, string>
        {
            { typeof(Zone_RandomDungeon), "Zone_RandomDungeon" },
            { typeof(ZoneEvent), "ZoneEvent" },
        };

        internal static void DeployTypeFallbackSetting()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            var typeFallbackSetting = ReadTypeFallbackSetting();
            bool shouldFileUpdate = false;

            foreach (var targetNamespace in TargetNamespaceList)
            {
                var classes = assembly.GetTypes().Where(t => t.IsClass && t.Namespace == targetNamespace);

                foreach (var type in classes)
                {
                    string className = type.Name;
                    string baseClass = "";

                    foreach (var kvp in BaseClassResolver)
                    {
                        if (kvp.Key.IsAssignableFrom(type))
                        {
                            baseClass = kvp.Value;
                            break;
                        }
                    }

                    if (baseClass == "")
                    {
                        continue;
                    }

                    // "「アセンブリ名」,「名前空間.クラス名」, 「フォールバック先Elinクラス名」"
                    StringBuilder fallbackSB = new StringBuilder(110);
                    fallbackSB.Append(assembly.GetName().Name).Append(",");
                    fallbackSB.Append(targetNamespace).Append(String.IsNullOrEmpty(targetNamespace) ? "" : ".");
                    fallbackSB.Append(className).Append(",");
                    fallbackSB.Append(baseClass);

                    if (!typeFallbackSetting.Contains(fallbackSB.ToString()))
                    {
                        typeFallbackSetting.Add(fallbackSB.ToString());
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
