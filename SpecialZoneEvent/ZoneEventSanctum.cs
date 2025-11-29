using Newtonsoft.Json;

namespace SpecialNefia.SpecialZoneEvent
{
    class ZoneEventSanctum : ZoneEvent
    {
        [JsonProperty]
        Religion NefiaReligion;

        public ZoneEventSanctum(Religion ruleReligion)
        {
            NefiaReligion = ruleReligion;
        }

        public override void OnVisit()
        {
            foreach (var thing in EClass._zone.map.things)
            {
                if (thing.id == "altar" && EClass.rnd(5) != 0)
                {
                    thing.c_idDeity = NefiaReligion.id;
                    thing.ChangeMaterial(NefiaReligion.source.idMaterial);
                }
            }
        }
    }
}
