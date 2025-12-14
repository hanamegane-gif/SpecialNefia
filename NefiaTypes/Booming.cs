using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Booming : NefiaType, IConditionFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_booming".lang();

        public override int MinDangerLv => 20;

        public override int NefiaTypeOdds => 1;

        public override int RuleDescriptionId => 912004;

        public void TickPostfixAction(Chara target)
        {
            if (target == null)
            {
                return;
            }

            if (!target.IsPCFactionOrMinion && !target.HasCondition<ConBrightnessOfLife>())
            {
                target.AddCondition<ConBrightnessOfLife>(p: 19980, force: true);
            }
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IConditionFix;
        }
    }
}
