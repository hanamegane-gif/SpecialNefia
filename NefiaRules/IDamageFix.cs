using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaRules
{
    // ダメージの量をいじる
    interface IDamageFix : INefiaRule, IExclusiveRule
    {
        void DamageFixAction(Card target, ref long dmg, int ele, ref AttackSource attackSource);
    }
}
