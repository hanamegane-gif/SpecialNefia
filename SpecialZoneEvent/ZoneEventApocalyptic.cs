using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.SpecialZoneEvent
{
    class ZoneEventApocalyptic : ZoneEvent
    {
        [JsonProperty]
        public int ElapsedRounds = 0;

        public override void OnTickRound()
        {
            ElapsedRounds++;
            if ((ElapsedRounds + 1) % 5 == 0)
            {
                DoMeteor();
            }
        }

        private void DoMeteor()
        {
            int meteorPower = 100 + EClass.curve(this.zone.DangerLv * 6, 400, 100, 75);
            ActEffect.ProcAt(EffectId.Meteor, meteorPower, BlessedState.Normal, EClass.pc, null, EClass.pc.pos, isNeg: false);
        }
    }
}
