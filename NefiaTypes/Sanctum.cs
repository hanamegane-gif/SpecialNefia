using Newtonsoft.Json;
using SpecialNefia.NefiaRules;
using SpecialNefia.SpecialZoneEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Sanctum : NefiaType, IElementFix, IFloorEvent
    {
        [JsonProperty]
        private Religion NefiaReligion;

        public override int MinDangerLv => 1;

        public override string NefiaTypeName => (NefiaReligion?.NameShort ?? "?") + "byakko_mod_nefia_sanctum".lang();

        public override int RuleDescriptionId => 912020;

        // 基本x1だが速度が上がる信仰は倍率を上げる
        public override int NefiaTypeOdds
        {
            get
            {
                SourceReligion.Row row = EClass.sources.religions.map.TryGetValue(NefiaReligion.id);

                if (row == null)
                {
                    return 0;
                }

                if (row.elements.Where((e, i) => i % 2 == 0 && e == SKILL.SPD).Any())
                {
                    return (EClass._zone.DangerLv >= 5000 && !EClass.game.principal.disableVoidBlessing) ? 2 : 1;
                }

                return 0;
            }
        }
        public ZoneEvent GetFloorEvent()
        {
            return new ZoneEventSanctum(NefiaReligion);
        }

        public void SpawnMobPostfixAction(Chara spawned)
        {
            spawned.SetFaith(NefiaReligion);
        }

        public override NefiaType InitRule(Zone currentNefia)
        {
            NefiaReligion = EClass.game.religions.GetRandomReligion(onlyJoinable: true, includeMinor: true);

            return this;
        }
    }
}