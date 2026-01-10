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
    internal class Joust : NefiaType, IPartyRestriction
    {
        public override string NefiaTypeName => "byakko_mod_nefia_joust".lang();

        public override int MinDangerLv => 10;

        public override int NefiaTypeOdds
        {
            get
            {
                if (EClass.game.principal.disableVoidBlessing || EClass._zone.DangerLv < 400)
                {
                    return 1;
                }
                else
                {
                    return 2;
                }
            }
        }

        public override int RuleDescriptionId => 912013;

        [JsonProperty]
        private HashSet<int> _DismissedUID = new HashSet<int>();

        public ZoneEvent GetPartyRestrictingEvent()
        {
            return new ZoneEventJoust();
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
