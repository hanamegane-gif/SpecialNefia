using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class NefiaType
    {
        public virtual string NefiaTypeName => "？？？？";

        public virtual int MinDangerLv => 1;

        public virtual int NefiaTypeOdds => 0;

        internal virtual bool IsMeetRequirement(Zone_Dungeon nefia)
        {
            return MinDangerLv <= nefia.DangerLv;
        }

        internal bool IsExclusiveType(NefiaType nefiaType)
        {
            if (nefiaType is IExclusiveRule)
            {
                return (nefiaType as IExclusiveRule).HaveExclusiveRule(this);
            }

            return false;
        }

        public virtual NefiaType InitRule(Zone currentNefia)
        {
            return this;
        }
    }
}
