using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Heavy : NefiaType, IConditionFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_heavy".lang();

        public override int MinDangerLv => 1;

        public override int NefiaTypeOdds => 1;

        public void TickPostfixAction(Chara target)
        {
            if (target == null)
            {
                return;
            }

            if (!target.HasCondition<ConGravity>())
            {
                target.AddCondition<ConGravity>(p: 1800, force: true);
            }

            // 深層の拘束は操作不能になるレベルのため無効化する
            if (target.HasCondition<ConEntangle>())
            {
                target.RemoveCondition<ConEntangle>();
            }
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IConditionFix;
        }
    }
}
