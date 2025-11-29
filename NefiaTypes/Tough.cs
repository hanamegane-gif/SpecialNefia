using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Tough : NefiaType, IEnemyStrengthFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_tough".lang();

        public override int MinDangerLv => 10;

        public override int NefiaTypeOdds => 1;

        public void EnemyStrengrhFixAction(CardBlueprint blueprint)
        {
            blueprint.lv = blueprint.lv + (blueprint.lv / 4);
        }
    }
}
