using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal abstract class NefiaType
    {
        public abstract string NefiaTypeName { get; }

        public virtual int MinDangerLv => 1;

        public abstract int NefiaTypeOdds { get; }

        public abstract int RuleDescriptionId { get; }

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
