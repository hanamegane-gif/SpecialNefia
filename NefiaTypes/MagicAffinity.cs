using SpecialNefia.NefiaRules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.NefiaTypes
{
    internal class MagicAffinity : NefiaType, IElementFix
    {
        public override string NefiaTypeName => "byakko_mod_nefia_magicaffinity".lang();

        public override int MinDangerLv => 20;

        public override int NefiaTypeOdds => 1;

        public override int RuleDescriptionId => 912016;

        public void SpawnMobPostfixAction(Chara spawned)
        {
            spawned?.elements.ModBase(SKILL.antiMagic, -80);
        }
    }
}
