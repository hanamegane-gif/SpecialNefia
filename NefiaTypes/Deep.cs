using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Deep : NefiaType, IFloorNum
    {
        public override string NefiaTypeName => "byakko_mod_nefia_deep".lang();

        public override int MinDangerLv => 10;

        // 単純に長いため報酬多め
        public override int NefiaTypeOdds => 2;

        public int GetFloorNum(Zone nefia)
        {
            Rand.SetSeed(nefia.GetTopZone().uid);
            int result = -9 - EClass.rnd(3);
            Rand.SetSeed();
            return result;
        }

        public bool HaveExclusiveRule(NefiaType nefiaType)
        {
            return nefiaType is IFloorNum;
        }
    }
}
