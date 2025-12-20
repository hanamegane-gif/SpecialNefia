using BepInEx;
using Newtonsoft.Json;
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
    internal class Racial : NefiaType, ISpawnListFix
    {
        [JsonProperty]
        private SourceRace.Row SpawnRace;

        public override int MinDangerLv => 10;

        public override string NefiaTypeName => (SpawnRace?.GetName() ?? "?") + "byakko_mod_nefia_racial".lang();

        public override int RuleDescriptionId => 912019;

        private Dictionary<string, int> CandidatesCache = new Dictionary<string, int>();

        private int ChanceSumChache = 0;

        // 基本x1だがヤバめの種族は倍率を上げる
        public override int NefiaTypeOdds
        {
            get
            {
                if (SpawnRace.id == "piece" || SpawnRace.id == "imp" || SpawnRace.id == "machine" || SpawnRace.id == "quickling" || SpawnRace.id == "yith")
                {
                    return 1;
                }

                if (SpawnRace.id == "lich")
                {
                    return 2;
                }

                if (SpawnRace.id == "demon" || SpawnRace.id == "dragon")
                {
                    return 4;
                }

                return 0;
            }
        }

        public CardRow SpawnListFixAction(CardRow original, SpawnSetting setting)
        {
            if (!CandidatesCache.Any())
            {
                CandidatesCache = EClass.sources.charas.rows.Where(r => (r.hostility == "") && (r.quality < 4) && r.chance > 0 && r.race == SpawnRace.id)
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
            return nefiaType is ISpawnListFix;
        }

        public override NefiaType InitRule(Zone currentNefia)
        {
            int genLv = currentNefia.DangerLv;
            var candidates = new List<SourceRace.Row>();
            var races = EClass.sources.races.rows;

            foreach (var race in races)
            {
                var generatableChara = EClass.sources.charas.rows.Where(c => c.race == race.id && c.chance > 0 && c.hostility.IsNullOrWhiteSpace() && c.LV <= genLv);
                var generatableKinds = generatableChara.Sum(c => (c.mainElement.Count() == 0) ? 1 : c.mainElement.Count());
                if (generatableKinds >= 2)
                {
                    candidates.Add(race);
                }
            }

            SpawnRace = candidates.RandomItem();

            return this;
        }
    }
}
