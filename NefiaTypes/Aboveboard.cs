using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Weather;

namespace SpecialNefia.NefiaTypes
{
    internal class Aboveboard : NefiaType, IConditionFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_aboveboard".lang();

        public override int MinDangerLv => 1;

        // バフデバフ禁止は主に透明化リロ勢・ムンゲ勢に刺さる
        // 通常プレイでも猫の目とテレパシー封印はかなりキツイ
        public override int NefiaTypeOdds
        {
            get
            {
                if (EClass._zone.DangerLv < 200)
                {
                    return 1;
                }
                else if (EClass._zone.DangerLv < 800)
                {
                    return 2;
                }
                else
                {
                    return 4;
                }
            }
        }

        public override int RuleDescriptionId => 912000;

        public void TickPostfixAction(Chara target)
        {
            if (target == null)
            {
                return;
            }

            // 弱体化OFは時間掛けてるとは思うが処す
            target.ClearTempElements();

            // OF回復によってHP不正値だと無敵なのでこれも処す
            if (target.hp < 0)
            {
                target.hp = 0;
            }

            for (int i = target.conditions.Count - 1; i >= 0; i--)
            {
                Condition condition = target.conditions[i];
                switch (condition.Type)
                {
                    case ConditionType.Buff:
                        if (!condition.isPerfume && condition is not ConRebirth)
                        {
                            condition.Kill(silent: true);
                        }
                        break;
                    case ConditionType.Neutral:
                        if (condition is ConSupress)
                        {
                            condition.Kill(silent: true);
                        }
                        break;
                    case ConditionType.Bad:
                        if (condition is ConSleep && target.IsPC)
                        {
                            if ((condition as ConSleep).pcSleep == 0)
                            {
                                condition.Kill(silent: true);
                            }
                        }
                        else if (condition is not ConFaint)
                        {
                            condition.Kill(silent: true);
                        }
                        break;
                    case ConditionType.Debuff:
                        condition.Kill(silent: true);
                        break;
                    case ConditionType.Disease:
                        if (condition is not ConAnorexia)
                        {
                            condition.Kill(silent: true);
                        }
                        break;
                }
            }
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IConditionFix;
        }
    }
}
