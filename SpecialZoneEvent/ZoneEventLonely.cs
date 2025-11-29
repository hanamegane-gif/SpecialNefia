using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;

namespace SpecialNefia.SpecialZoneEvent
{
    class ZoneEventLonely : ZoneEvent
    {
        [JsonProperty]
        public HashSet<int> DismissedUID = null;

        public override void OnVisit()
        {
            if (DismissedUID == null)
            {
                DismissedUID = new HashSet<int>();
            }

            bool dismissed = false;
            for (int i = EClass.pc.party.members.Count - 1; i >= 0; i--)
            {
                Chara member = EClass.pc.party.members[i];
                if (member != EClass.pc)
                {
                    DismissedUID.Add(member.uid);
                    EClass.pc.party.RemoveMember(member);
                    member.MoveZone(member.homeZone);
                    dismissed = true;
                }
            }

            if (dismissed)
            {
                EClass.pc.Say("byakko_mod_nefia_party_restricting_lonely");
            }
        }

        public override void OnLeaveZone()
        {
            if (EClass.player.nextZone.lv < 0)
            {
                return;
            }

            foreach (var UID in DismissedUID)
            {
                var chara = EClass.game.cards.globalCharas.Where((KeyValuePair<int, Chara> a) => a.Value.uid == UID).FirstOrDefault().Value;
                EClass._zone.AddCard(chara, EClass.pc.pos.GetNearestPoint());
                EClass.pc.party.AddMemeber(chara, showMsg: false);
            }
        }
    }
}
