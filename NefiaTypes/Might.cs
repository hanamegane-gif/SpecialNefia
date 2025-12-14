using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Might : NefiaType, IDamageFix, ISpawnListFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_might".lang();

        public override int MinDangerLv => 30;

        public override int NefiaTypeOdds => 2;

        public override int RuleDescriptionId => 912017;

        private HashSet<string> JobList { get; } = new HashSet<string>
        {
            "warrior", "thief", "archer", "gunner", "inquisitor", "paladin", "executioner", "predator", "defender", "none"
        };
        
        private HashSet<string> CandidatesCache = new HashSet<string>();

        public void DamageFixAction(Card target, ref long dmg, int ele, ref AttackSource attackSource)
        {
            if(!target.isChara)
            {
                return;
            }

            if (attackSource == AttackSource.None ||
                attackSource == AttackSource.MagicArrow ||
                attackSource == AttackSource.MagicHand ||
                attackSource == AttackSource.MagicSword ||
                attackSource == AttackSource.MoonSpear
               )
            {
                if (!target.IsPCFaction && EClass.rnd(4) == 0)
                {
                    target.Say("byakko_mod_nefia_might_nodamage");
                }
                dmg = 0;
                return;
            }
        }

        public CardRow SpawnListFixAction(CardRow original, SpawnSetting setting)
        {
            if (!CandidatesCache.Any())
            {
                CandidatesCache = EClass.sources.charas.rows.Where(r => (r.hostility == "") && (r.quality < 4) && r.chance > 0 && JobList.Contains(r.job))
                                        .Select(r => r.id).ToHashSet();
            }

            if (EClass.rnd(4) != 0 || setting.isBoss)
            {
                var spawnID = CandidatesCache.RandomItem();
                return EClass.sources.charas.rows.Find(r => r.id == spawnID);
            }
            else
            {
                return original;
            }
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return (nefiaType is IDamageFix) || (nefiaType is ISpawnListFix);
        }
    }
}
