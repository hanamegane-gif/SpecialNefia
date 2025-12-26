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
    internal class Lonely : NefiaType, IPartyRestriction
    {
        public override string NefiaTypeName => "byakko_mod_nefia_lonely".lang();

        public override int MinDangerLv => 10;

        [JsonProperty]
        private HashSet<int> _DismissedUID = new HashSet<int>();

        // 加護あり深層完ソロできる人にはちゃんと報酬を出す
        public override int NefiaTypeOdds
        {
            get
            {
                if (EClass.game.principal.disableVoidBlessing || EClass.game.principal.enableDamageReduction || EClass._zone.DangerLv < 1500)
                {
                    return 2;
                }

                if (EClass._zone.DangerLv >= 8000)
                {
                    return 6 + ((EClass.game.principal.disableManualSave && !EClass.game.principal.modified.Contains("disableManualSave")) ? 8 : 0);
                }
                else
                {
                    return 4 + ((EClass.game.principal.disableManualSave && !EClass.game.principal.modified.Contains("disableManualSave")) ? 3 : 0);
                }
            }
        }

        public override int RuleDescriptionId => 912014;

        public ZoneEvent GetPartyRestrictingEvent()
        {
            return new ZoneEventLonely();
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IPartyRestriction;
        }

        public HashSet<int> GetDismissedCharas()
        {
            return _DismissedUID;
        }

        public void AddDismissedChara(int uid)
        {
            _DismissedUID.Add(uid);
        }

        public void ClearDismissedChara()
        {
            _DismissedUID = new HashSet<int>();
        }
    }
}
