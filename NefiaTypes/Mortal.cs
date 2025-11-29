using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static BiomeProfile;

namespace SpecialNefia.NefiaTypes
{
    internal class Mortal : NefiaType, IDamageFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_mortal".lang();

        public override int MinDangerLv => 20;

        public override int NefiaTypeOdds => 1;

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

            // OF回復によってHP不正値だと無敵なので処す
            if (target.hp < 0)
            {
                target.hp = 0;
            }

            dmg = target.MaxHP + ((target.Evalue(FEAT.featManaMeat) > 0) ? (target as Chara).mana.max : 0);
            dmg = dmg / (1 + (GetCurrentLife(target) / 20)) + 1;
            attackSource = AttackSource.Finish;
        }

        // ボス補正・孤独補正付き生命力を取得
        public int GetCurrentLife(Card target)
        {
            int rarityMultiplier = (target.IsPCFactionOrMinion) ? 0 : ((int)target.rarity * 300);
            int lonelyMultiplier = (target.IsPC) ? (EClass.player.lastEmptyAlly * target.Evalue(1646)) : 0;
            int life = target.Evalue(SKILL.life) * (100 + rarityMultiplier + lonelyMultiplier) / 100;

            return Mathf.Max(life, 0);
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IDamageFix;
        }
    }
}
