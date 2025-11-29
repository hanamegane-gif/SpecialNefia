using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaRules
{
    // DamageHP処理前に何かする
    interface IDamagePrefix : INefiaRule
    {
        void DamageHPPrefixAction(Card target, Card origin);
    }
}
