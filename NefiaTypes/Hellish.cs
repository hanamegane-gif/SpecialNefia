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

        public override int NefiaTypeOdds => 4;

        public override int RuleDescriptionId => 912011;

        private HashSet<string> CandidatesCache = new HashSet<string>();

        public CardRow SpawnListFixAction(CardRow original, SpawnSetting setting)
        {
            if (!CandidatesCache.Any())
            {
                CandidatesCache = EClass.sources.charas.rows.Where(r => (r.hostility == "") && (r.quality < 4) && r.chance > 0 && (r.race == "dragon" || r.race == "demon" || r.race == "giant"))
                                        .Select(r => r.id).ToHashSet();
            }
            var spawnID = CandidatesCache.RandomItem();
            return EClass.sources.charas.rows.Find(r => r.id == spawnID);
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is ISpawnListFix;
        }
    }
}
