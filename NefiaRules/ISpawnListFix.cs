using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaRules
{
    interface ISpawnListFix : INefiaRule, IExclusiveRule
    {
        CardRow SpawnListFixAction(CardRow original, SpawnSetting setting);
    }
}
