using HarmonyLib;
using Newtonsoft.Json;
using SpecialNefia.Config;
using SpecialNefia.Nefia;
using SpecialNefia.NefiaRules;
using SpecialNefia.NefiaTypes;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class ZoneSpecialNefia : Zone_RandomDungeon, ISpecialNefia
{
    public override string Name => GetSpecialTypeName() + Lang.space + ((idPrefix == 0) ? "" : (EClass.sources.zoneAffixes.map[idPrefix].GetName().ToTitleCase() + Lang.space)) + name.IsEmpty(source.GetText()) + NameSuffix;

    [JsonProperty]
    private List<NefiaType> _NefiaTypes = new List<NefiaType>();

    [JsonProperty]
    private string ModZoneId = "";

    [JsonProperty]
    private string VanillaZoneId = "";

    public override int LvBoss => GetFloorNum();

    public override string GetDungenID()
    {
        return GetDungeonID();
    }

    internal IFloorNum FloorNumRule => _NefiaTypes.Where(nt => nt is IFloorNum).Cast<IFloorNum>().FirstOrDefault();

    internal ISpeedFix SpeedFixRule => _NefiaTypes.Where(nt => nt is ISpeedFix).Cast<ISpeedFix>().FirstOrDefault();

    internal IGenMap GenMapRule => _NefiaTypes.Where(nt => nt is IGenMap).Cast<IGenMap>().FirstOrDefault();

    internal IDamageFix DamageFixRule => _NefiaTypes.Where(nt => nt is IDamageFix).Cast<IDamageFix>().FirstOrDefault();

    internal ISpawnListFix SpawnListRule => _NefiaTypes.Where(nt => nt is ISpawnListFix).Cast<ISpawnListFix>().FirstOrDefault();

    internal IPartyRestriction PartyRestrictionRule => _NefiaTypes.Where(nt => nt is IPartyRestriction).Cast<IPartyRestriction>().FirstOrDefault();

    internal IEnumerable<IElementFix> FixElementRules => _NefiaTypes.Where(nt => nt is IElementFix).Cast<IElementFix>();

    internal IEnumerable<IConditionFix> FixConditionRules => _NefiaTypes.Where(nt => nt is IConditionFix).Cast<IConditionFix>();

    internal IEnumerable<IDamagePrefix> DamagePrefixRules => _NefiaTypes.Where(nt => nt is IDamagePrefix).Cast<IDamagePrefix>();

    internal IEnumerable<IDamagePostfix> DamagePostfixRules => _NefiaTypes.Where(nt => nt is IDamagePostfix).Cast<IDamagePostfix>();

    internal IEnumerable<IEnemyStrengthFix> EnemyStrengthFixRules => _NefiaTypes.Where(nt => nt is IEnemyStrengthFix).Cast<IEnemyStrengthFix>();

    internal IEnumerable<IFloorEvent> FloorEventRules => _NefiaTypes.Where(nt => nt is IFloorEvent).Cast<IFloorEvent>();

    // これは階層移動時に生成されるZoneのidとして使われる
    // Mod外した際のセーブデータ破壊を防ぐため保存するidはバニラのidである必要がある
    public override string GetNewZoneID(int destLv)
    {
        return this.ModZoneId;
    }

    public void Init()
    {
        InitSpecialType();
        RevertToVanillaZoneId();
        AddRuleFloorEvents();
        AddPartyRestrictingEvent();
    }

    public void InitSpecialType()
    {
        if (this.lv < -1)
        {
            InheritParentNefiaType();
        }
        else
        {
            _NefiaTypes = new List<NefiaType>();
            var types = NefiaTypeFactory.CreateRandomNefiaTypes(this, 1 + EClass.rndSqrt(2) + EClass.rndSqrt(2));
            foreach (var t in types)
            {
                TryAddNefiaType(t.InitRule(this));
            }
        }
    }

    public bool TryAddNefiaType(NefiaType nefiaType)
    {
        foreach (var item in _NefiaTypes)
        {
            if (nefiaType.GetType() == item.GetType() || item.IsExclusiveType(nefiaType))
            {
                return false;
            }
        }

        _NefiaTypes.Add(nefiaType);
        return true;
    }

    public void InheritParentNefiaType()
    {
        this._NefiaTypes = (this.GetTopZone() as ISpecialNefia).GetSpecialTypes();
    }

    public List<NefiaType> GetSpecialTypes()
    {
        return _NefiaTypes ?? new List<NefiaType>(); 
    }

    public int GetNefiaTypeOdds()
    {
        return 1 + _NefiaTypes.Select(t => t.NefiaTypeOdds).Sum();
    }

    public string GetSpecialTypeName()
    {
        return String.Join(Lang.space, _NefiaTypes.Select(nt => nt.NefiaTypeName).ToArray());
    }

    public int GetFloorNum()
    {
        return FloorNumRule?.GetFloorNum(this) ?? base.LvBoss;
    }

    public int GetSpeedFix(Chara c, int speedMul, Element.BonusInfo info)
    {
        return SpeedFixRule?.GetSpeedFix(c, speedMul, info) ?? speedMul;
    }

    public string GetDungeonID()
    {
        return GenMapRule?.GetDungeonID() ?? base.GetDungenID();
    }

    public void InvokeDamageFixAction(Card target, ref long dmg, int ele, ref AttackSource attackSource)
    {
        DamageFixRule?.DamageFixAction(target, ref dmg, ele, ref attackSource);
    }

    public CardRow InvokeSpawnListFixAction(CardRow original, SpawnSetting setting)
    {
        return SpawnListRule?.SpawnListFixAction(original, setting) ?? original;
    }

    public void InvokeSpawnsPostfixActions(Chara spawned)
    {
        foreach (var rule in FixElementRules)
        {
            rule.SpawnMobPostfixAction(spawned);
        }
    }

    public void InvokeTickPostfixActions(Chara target)
    {
        foreach (var rule in FixConditionRules)
        {
            rule.TickPostfixAction(target);
        }
    }

    public void InvokeDamageHPPrefixActions(Card target, Card origin)
    {
        foreach (var rule in DamagePrefixRules)
        {
            rule.DamageHPPrefixAction(target, origin);
        }
    }

    public void InvokeDamageHPPostfixActions(Card target, Card origin)
    {
        foreach (var rule in DamagePostfixRules)
        {
            rule.DamageHPPostfixAction(target, origin);
        }
    }

    public void InvokeEnemyStrengthFixActions(CardBlueprint blueprint)
    {
        foreach (var rule in EnemyStrengthFixRules)
        {
            rule.EnemyStrengrhFixAction(blueprint);
        }
    }

    public void AddRuleFloorEvents()
    {
        foreach (var rule in FloorEventRules)
        {
            this.events.Add(rule.GetFloorEvent());
        }
    }

    public void AddPartyRestrictingEvent()
    {
        if (PartyRestrictionRule != null)
        {
            this.events.Add(PartyRestrictionRule.GetPartyRestrictingEvent());
        }
    }

    public void SpawnRewardChests(Chara boss)
    {
        int odds = GetNefiaTypeOdds();

        // 容量の問題と演出のため追加宝箱の中身は分けて出す
        for (int i = 0; i < odds; i++)
        {
            Thing rewardChest = ThingGen.Create("chest_boss", lv: 1);
            rewardChest.things.Clear();
            rewardChest.c_lockLv = 0;

            rewardChest.Add("plat", EClass.rndHalf(3 + EClass.curve(boss.LV / 20, 5, 15, 60)));
            rewardChest.Add("medal", EClass.rnd(3));

            for (int j = 0; j < 2; j++)
            {
                rewardChest.AddThing(GenerateEquipment());
            }

            if (ModConfig.EnableLawlessRuneFeature)
            {
                if (EClass.rnd(2) == 0)
                {
                    rewardChest.Add("MOD_byakko_SPN_rune_shard_free", 1);
                }

                if (this.DangerLv > 2000)
                {
                    rewardChest.Add("MOD_byakko_SPN_rune_shard_free", 1);
                }
            }

            EClass._zone.AddCard(rewardChest, boss.pos.GetNearestPoint(allowInstalled: false, minRadius: 1)).Install();
        }

        Thing GenerateEquipment()
        {
            CardBlueprint.Set(new CardBlueprint
            {
                rarity = (EClass.rnd(5) == 0) ? Rarity.Mythical : Rarity.Legendary
            });
            return ThingGen.CreateFromFilter("eq", boss.LV);
        }
    }

    public void RevertToVanillaZoneId()
    {
        this.ModZoneId = String.Copy(this.id);
        this.VanillaZoneId = this.ModZoneId.Replace("byakko_mod_spn_", "");
        this.id = String.Copy(this.VanillaZoneId);
    }
}
