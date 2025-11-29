using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Empathetic : NefiaType, IConditionFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_empathetic".lang();

        public override int MinDangerLv => 1;

        public override int NefiaTypeOdds => 1;

        public void TickPostfixAction(Chara target)
        {
            if (target == null)
            {
                return;
            }

            if (!target.HasCondition<ConTelepathy>())
            {
                target.AddCondition<ConTelepathy>(p: 425, force: true);
            }
            if (!target.HasCondition<ConSeeInvisible>())
            {
                target.AddCondition<ConSeeInvisible>(p: 528, force: true);
            }
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IConditionFix;
        }
    }
}
