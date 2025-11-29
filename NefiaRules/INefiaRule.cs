using SpecialNefia.Nefia;
using SpecialNefia.NefiaTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaRules
{
    interface INefiaRule
    {
        NefiaType InitRule(Zone currentNefia);
    }
}
