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

        public override int NefiaTypeOdds => 1;

        public ZoneEvent GetPartyRestrictingEvent()
        {
            return new ZoneEventJoust();
        }


        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IPartyRestriction;
        }
    }
}
