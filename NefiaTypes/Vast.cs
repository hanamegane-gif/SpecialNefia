using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Vast : NefiaType, IGenMap
    {
        public override string NefiaTypeName => "byakko_mod_nefia_vast".lang();

        public override int MinDangerLv => 1;

        // x1でもいいかも？
        public override int NefiaTypeOdds => 1;

        public string GetDungeonID()
        {
            return "CavernBig";
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IGenMap;
        }
    }
}
