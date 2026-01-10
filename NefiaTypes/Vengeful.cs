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

        public override int RuleDescriptionId => 912024;

        // 攻撃者不明のダメージで倒すと殺害数にカウントされないためクエストフラグを直接見る
        // デュポンヌとかはそもそも交戦すらしない
        private Dictionary<string, Func<bool>> BossFlagQuestDict { get; } = new Dictionary<string, Func<bool>>
        {
            {"isygarad", () => EClass.game.quests.IsCompleted("exploration") },
            {"mech_golem_a", () => EClass.game.quests.IsCompleted("exploration") },
            {"mech_golem_b", () => EClass.game.quests.IsCompleted("exploration") },
            {"ungaga_pap", () => EClass.game.quests.IsCompleted("negotiation_darkness") },
            {"melilith_boss", () => EClass.game.quests.IsCompleted("melilith") },
            {"vernis_boss", () => EClass.game.quests.IsCompleted("vernis_gold") },
            {"lurie_boss", () => EClass.game.quests.IsCompleted("negotiation_darkness") },
            {"doga", () => EClass.game.quests.IsCompleted("curry") },
            {"azzrasizzle", () => EClass.game.quests.IsCompleted("into_darkness") || EClass.game.quests.GetPhase<QuestIntoDarkness>() > 5 },
        };


        private HashSet<string> CandidatesCache = new HashSet<string>();

        public CardRow SpawnListFixAction(CardRow original, SpawnSetting setting)
        {
            if (!CandidatesCache.Any())
            {
                CandidatesCache = EClass.sources.charas.rows.Where(r => IsSpawnableBoss(r.id))
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

        private bool IsSpawnableBoss(string id)
        {
            return BossFlagQuestDict.ContainsKey(id) && BossFlagQuestDict[id]();
        }

        private bool IsExistSpawnableBoss()
        {
            foreach (var kvp in BossFlagQuestDict)
            {
                if (BossFlagQuestDict[kvp.Key]())
                {
                    return true;
                }
            }

            return false;
        }
    }
}
