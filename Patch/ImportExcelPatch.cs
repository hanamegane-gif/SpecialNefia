using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.Patch
{
    class ImportExcelPatch
    {
        const string __MOD_SOURCE_DIR__ = "LangMod/JP";

        internal static void ExecImportLanguageGame(in PluginInfo info)
        {
            var dir = Path.Combine(Path.GetDirectoryName(info.Location), __MOD_SOURCE_DIR__);
            var excel = Path.Combine(dir, "LangGame.xlsx");
            var sources = Core.Instance.sources;
            ModUtil.ImportExcel(excel, "Game", sources.langGame);
        }

        internal static void ExecImportLanguageGeneral(in PluginInfo info)
        {
            var dir = Path.Combine(Path.GetDirectoryName(info.Location), __MOD_SOURCE_DIR__);
            var excel = Path.Combine(dir, "LangGeneral.xlsx");
            var sources = Core.Instance.sources;
            ModUtil.ImportExcel(excel, "General", sources.langGeneral);
        }

        internal static void ExecImportZones(in PluginInfo info)
        {
            var dir = Path.Combine(Path.GetDirectoryName(info.Location), __MOD_SOURCE_DIR__);
            var excel = Path.Combine(dir, "Zone.xlsx");
            var sources = Core.Instance.sources;
            ModUtil.ImportExcel(excel, "Zone", sources.zones);
        }

        internal static void ExecImportThings(in PluginInfo info)
        {
            var dir = Path.Combine(Path.GetDirectoryName(info.Location), __MOD_SOURCE_DIR__);
            var excel = Path.Combine(dir, "Thing.xlsx");
            var sources = Core.Instance.sources;
            ModUtil.ImportExcel(excel, "Thing", sources.things);
        }
    }
}
