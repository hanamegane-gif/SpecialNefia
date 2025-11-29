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

        public void EnemyStrengrhFixAction(CardBlueprint blueprint)
        {
            if (blueprint.rarity == Rarity.Normal && EClass.rnd(4) == 0)
            {
                blueprint.rarity = Rarity.Legendary;
                blueprint.lv = blueprint.lv + (blueprint.lv / 4);
            }
        }
    }
}
