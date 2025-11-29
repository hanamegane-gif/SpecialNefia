using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Illusion : NefiaType, IConditionFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_illusion".lang();

        public override int MinDangerLv => 1;

        public override int NefiaTypeOdds => 1;

        public void TickPostfixAction(Chara target)
        {
            if (target == null)
            {
                return;
            }

            if (!target.HasCondition<ConHallucination>())
            {
                target.RemoveCondition<ConPeace>();
                target.AddCondition<ConHallucination>(p: 100, force: true);
            }
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IConditionFix;
        }
    }
}
