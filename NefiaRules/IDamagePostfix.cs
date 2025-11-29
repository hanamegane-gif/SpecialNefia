using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaRules
{
    // DamageHP処理後に何かする
    interface IDamagePostfix : INefiaRule
    {
        void DamageHPPostfixAction(Card target, Card origin);
    }
}
