using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaRules
{
    interface ISpeedFix : INefiaRule, IExclusiveRule
    {
        int GetSpeedFix(Chara c, int speedMul, Element.BonusInfo info);
    }
}
