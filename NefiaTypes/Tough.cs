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

        public override int RuleDescriptionId => 912022;

        public void EnemyStrengrhFixAction(CardRow original, CardBlueprint blueprint)
        {
            int originalLv = (original.LV > blueprint.lv) ? original.LV : blueprint.lv;
            blueprint.lv = originalLv + (originalLv / 4);
        }
    }
}
