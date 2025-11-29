using SpecialNefia.NefiaRules;
using SpecialNefia.SpecialZoneEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Apocalyptic : NefiaType, INefiaRule, IFloorEvent
    {
        public override string NefiaTypeName => "byakko_mod_nefia_apocalyptic".lang();

        public override int MinDangerLv => 30;

        public override int NefiaTypeOdds => 1;

        public ZoneEvent GetFloorEvent()
        {
            return new ZoneEventApocalyptic();
        }
    }
}
