using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Fair : NefiaType, ISpeedFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_fair".lang();

        public override int MinDangerLv => 1;

        public override int NefiaTypeOdds => 1;

        public override int RuleDescriptionId => 912008;

        public int GetSpeedFix(Chara c, int speedMul, Element.BonusInfo info)
        {
            info?.AddFix(-(speedMul), "byakko_mod_nefia_fair_speedfix".lang());
            return 0;
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is ISpeedFix;
        }
    }
}
