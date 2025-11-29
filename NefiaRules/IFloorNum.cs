using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaRules
{
    internal interface IFloorNum : INefiaRule, IExclusiveRule
    {
        int GetFloorNum(Zone nefia);
    }
}
