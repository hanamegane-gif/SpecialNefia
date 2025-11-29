using BepInEx;
using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.Config
{
    class ModConfig
    {
        const string __CONFIG_FILE_NAME__ = "byakko.elin.SpecialNefia.cfg";

        private static ConfigFile CustomConfig;
        private static ConfigEntry<int> _SpecialNefiaChance;
        private static ConfigEntry<bool> _LawlessRuneFeature;

        internal static int SpecialNefiaChance { get => _SpecialNefiaChance.Value; }

        internal static bool EnableLawlessRuneFeature { get => _LawlessRuneFeature.Value; }

        internal static void LoadConfig()
        {
            string configPath = Path.Combine(Paths.ConfigPath, __CONFIG_FILE_NAME__);
            CustomConfig = new ConfigFile(configPath, true);

            _SpecialNefiaChance = CustomConfig.Bind
            (
                "General",
                "SpecialNefiaChance",
                20,
                "chance of generating a special nefia(%): 特殊ネフィア生成率(%)"
            );

            _LawlessRuneFeature = CustomConfig.Bind
            (
                "General",
                "EnableLawlessRuneFeature",
                true,
                "If false, disables all lawless rune-related functions.: falseにすると無法ルーン関連の機能を無効化します。"
            );
        }
    }
}
