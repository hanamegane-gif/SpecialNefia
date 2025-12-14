using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class Ancient : NefiaType, IDamagePrefix, IDamagePostfix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_ancient".lang();

        public override int MinDangerLv => 150;

        public override int NefiaTypeOdds => 2;

        public override int RuleDescriptionId => 912001;

        public void DamageHPPrefixAction(Card target, Card origin)
        {
            if (origin != null && origin.isChara)
            {
                origin.elements.ModBase(FEAT.featElder, 1);
            }
        }

        public void DamageHPPostfixAction(Card target, Card origin)
        {
            if (origin != null && origin.isChara)
            {
                origin.elements.ModBase(FEAT.featElder, -1);
            }
        }
    }
}
