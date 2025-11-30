using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Fearsome : NefiaType, IEnemyStrengthFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_fearsome".lang();

        public override int MinDangerLv => 20;

        public override int NefiaTypeOdds => 1;

        public void EnemyStrengrhFixAction(CardRow original, CardBlueprint blueprint)
        {
            if (blueprint.rarity == Rarity.Normal && EClass.rnd(4) == 0)
            {
                int originalLv = (original.LV > blueprint.lv) ? original.LV : blueprint.lv;
                blueprint.rarity = Rarity.Legendary;
                blueprint.lv = originalLv + (originalLv / 4);
            }
        }
    }
}
