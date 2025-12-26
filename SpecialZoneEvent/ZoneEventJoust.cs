using Newtonsoft.Json;
using SpecialNefia.Nefia;
using SpecialNefia.NefiaTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.SpecialZoneEvent
{
    class ZoneEventJoust : ZoneEvent
    {
        [JsonProperty]
        public HashSet<int> DismissedUID = null;

        public override void OnVisit()
        {
            var nt = (this.zone as ISpecialNefia).GetSpecialTypes().Find(t => (t is Joust)) as Joust;

            bool dismissed = false;
            for (int i = EClass.pc.party.members.Count - 1; i >= 0; i--)
            {
                Chara member = EClass.pc.party.members[i];
                if (member != EClass.pc && member != EClass.pc.ride && member != EClass.pc.parasite)
                {
                    nt.AddDismissedChara(member.uid);
                    EClass.pc.party.RemoveMember(member);
                    member.MoveZone(member.homeZone);
                    dismissed = true;
                }
            }

            if (dismissed)
            {
                EClass.pc.Say("byakko_mod_nefia_party_restricting_joust");
            }
        }

        public override void OnLeaveZone()
        {
            if (EClass.player.nextZone.lv < 0)
            {
                return;
            }

            var nt = (this.zone as ISpecialNefia).GetSpecialTypes().Find(t => (t is Joust)) as Joust;
            foreach (var UID in nt.GetDismissedCharas())
            {
                var chara = EClass.game.cards.globalCharas.Where((KeyValuePair<int, Chara> a) => a.Value.uid == UID).FirstOrDefault().Value;
                EClass._zone.AddCard(chara, EClass.pc.pos.GetNearestPoint());
                EClass.pc.party.AddMemeber(chara, showMsg: false);
            }

            nt.ClearDismissedChara();
        }
    }
}
