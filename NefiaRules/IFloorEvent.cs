using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaRules
{
    interface IFloorEvent : INefiaRule
    {
        ZoneEvent GetFloorEvent();
    }
}
