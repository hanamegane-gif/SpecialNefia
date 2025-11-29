using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Shallow : NefiaType, IFloorNum
    {
        public override string NefiaTypeName => "byakko_mod_nefia_shallow".lang();

        public override int MinDangerLv => 1;

        public override int NefiaTypeOdds => 0;

        public int GetFloorNum(Zone nefia)
        {
            return -1;
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IFloorNum;
        }
    }
}
