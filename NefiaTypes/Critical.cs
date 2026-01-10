using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace SpecialNefia.NefiaTypes
{
    internal class Critical : NefiaType, IDamageFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_critical".lang();

        public override int MinDangerLv => 200;

        // 魔法が優位に、ただしタンクが機能しなくなるため難易度は高い
        public override int NefiaTypeOdds => 2;

        public override int RuleDescriptionId => 912005;

        public void DamageFixAction(Card target, ref long dmg, int ele, ref AttackSource attackSource)
        {
            if (!target.isChara)
            {
                return;
            }

            if (dmg == 0 || target.ResistLvFrom(ele) >= 4)
            {
                dmg = 0;
                return;
            }

            target.hp = 0;
            if (target.Evalue(FEAT.featManaMeat) > 0)
            {
                (target as Chara).mana.value = 0;
            }
            dmg = 99999999;
            attackSource = AttackSource.Finish;
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IDamageFix;
        }
    }
}
