using SpecialNefia.NefiaTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecialNefia.Nefia
{
    interface ISpecialNefia
    {        
        string GetSpecialTypeName();

        int GetFloorNum();

        int GetSpeedFix(Chara c, int speedMul, Element.BonusInfo info);

        string GetDungeonID();

        int GetNefiaTypeOdds();

        List<NefiaType> GetSpecialTypes();

        void Init();

        void InitSpecialType();

        bool TryAddNefiaType(NefiaType nefiaType);

        void InheritParentNefiaType();

        void InvokeDamageFixAction(Card target, ref long dmg, int ele, ref AttackSource attackSource);

        CardRow InvokeSpawnListFixAction(CardRow original, SpawnSetting setting);

        void InvokeSpawnsPostfixActions(Chara spawned);

        void InvokeTickPostfixActions(Chara target);

        void InvokeDamageHPPrefixActions(Card target, Card origin);

        void InvokeDamageHPPostfixActions(Card target, Card origin);

        void InvokeEnemyStrengthFixActions(CardRow original, CardBlueprint blueprint);

        void AddRuleFloorEvents();

        void AddPartyRestrictingEvent();

        void AddRuleDescription();

        void SpawnRewardChests(Chara boss);

        // Mod削除時のフォールバックでセーブデータが破壊されるためバニラidに偽装する
        void RevertToVanillaZoneId();
    }
}
