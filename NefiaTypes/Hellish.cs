using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BiomeProfile;

namespace SpecialNefia.NefiaTypes
{
    internal class Hellish : NefiaType, ISpawnListFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_hellish".lang();

        public override int MinDangerLv => 200;

        public override int NefiaTypeOdds => 3;

        public override int RuleDescriptionId => 912011;

        private Dictionary<string, int> CandidatesCache = new Dictionary<string, int>();

        private int ChanceSumChache = 0;

        public CardRow SpawnListFixAction(CardRow original, SpawnSetting setting)
        {
            if (!CandidatesCache.Any())
            {
                CandidatesCache = EClass.sources.charas.rows.Where(r => (r.hostility == "") && (r.quality < 4) && r.chance > 0 && (r.race == "dragon" || r.race == "demon" || r.race == "giant"))
                                        .ToDictionary(r => r.id, r => r.chance);
                ChanceSumChache = CandidatesCache.Values.Sum();
            }
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

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is ISpawnListFix;
        }
    }
}
