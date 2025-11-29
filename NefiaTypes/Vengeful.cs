using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    class Vengeful : NefiaType, ISpawnListFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_vengeful".lang();

        public override int MinDangerLv => 100;

        public override int NefiaTypeOdds => 1;

        // 攻撃者不明のダメージで倒すと殺害数にカウントされないためクエストフラグを直接見る
        // デュポンヌとかはそもそも交戦すらしない
        private Dictionary<string, string> BossFlagQuestDict { get; } = new Dictionary<string, string>
        {
            {"isygarad", "exploration"},
            {"ungaga_pap", "negotiation_darkness"},
            {"melilith_boss", "melilith"},
            {"vernis_boss", "vernis_gold"},
            {"lurie_boss", "negotiation_darkness"},
            {"doga", "curry"},
        };

        private HashSet<string> CandidatesCache = new HashSet<string>();

        public CardRow SpawnListFixAction(CardRow original, SpawnSetting setting)
        {
            if (!CandidatesCache.Any())
            {
                CandidatesCache = EClass.sources.charas.rows.Where(r => BossFlagQuestDict.ContainsKey(r.id) && IsSpawnable(r.id))
                                        .Select(r => r.id).ToHashSet();
            }

            if (setting.isBoss)
            {
                var spawnID = CandidatesCache.RandomItem();
                return EClass.sources.charas.rows.Find(r => r.id == spawnID);
            }
            else
            {
                return original;
            }
        }

        internal override bool IsMeetRequirement(Zone_Dungeon nefia)
        {
            return MinDangerLv <= nefia.DangerLv && IsExistSpawnableBoss();
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is ISpawnListFix;
        }

        private bool IsSpawnable(string id)
        {
            return EClass.game.quests.IsCompleted(BossFlagQuestDict.TryGetValue(id));
        }

        private bool IsExistSpawnableBoss()
        {
            foreach (var kvp in BossFlagQuestDict)
            {
                if (EClass.game.quests.IsCompleted(kvp.Value))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
