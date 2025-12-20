using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Magic : NefiaType, IDamageFix, ISpawnListFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_magic".lang();

        public override int MinDangerLv => 30;

        public override int NefiaTypeOdds => 2;

        public override int RuleDescriptionId => 912015;

        private HashSet<string> JobList { get; } = new HashSet<string>
        {
            "wizard", "warmage", "pianist", "priest", "witch", "swordsage", "bard", "alchemist", "none"
        };

        private Dictionary<string, int> CandidatesCache = new Dictionary<string, int>();

        private int ChanceSumChache = 0;

        public void DamageFixAction(Card target, ref long dmg, int ele, ref AttackSource attackSource)
        {
            if (!target.isChara)
            {
                return;
            }

            if (attackSource == AttackSource.Melee ||
                attackSource == AttackSource.Range ||
                attackSource == AttackSource.Throw ||
                attackSource == AttackSource.Shockwave
               )
            {
                if (!target.IsPCFaction && EClass.rnd(8) == 0)
                {
                    target.Say("byakko_mod_nefia_magic_nodamage");
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
                                        .ToDictionary(r => r.id, r => r.chance);
                ChanceSumChache = CandidatesCache.Values.Sum();
            }

            if (EClass.rnd(4) != 0 || setting.isBoss)
            {
                int roll = EClass.rnd(ChanceSumChache);
                int cumulative = 0;
                string spawnID = "";
                foreach (var kvp in CandidatesCache)
                {
                    cumulative += kvp.Value;
                    if (roll < cumulative)
                    {
                        spawnID = kvp.Key;
                        break;
                    }
                }
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
