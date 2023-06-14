using Common.Characters;
using UnityEngine;

namespace Common.Execution.Modify
{
    public class PowerModifier : ScriptableObject
    {
        // 모든 스킬의 쿨타임 줄이기
        // 특정 조건하 에서 공격력 높히기
        // 버프를 획득하기
        // public float PowerModify(float original, ICombatProvider provider, ICombatTaker taker)
        // {
        //     return Fortitude2Set(original, provider, taker);
        // }
        //
        // // Fortitude +2
        // public float Fortitude2Set(float originalPower, ICombatProvider provider, ICombatTaker taker)
        // {
        //     var providerForward = provider.gameObject.transform.forward;
        //     var takerForward    = taker.gameObject.transform.forward;
        //     var angle           = Vector3.Angle(providerForward, takerForward);
        //
        //     if (angle is <= 20.0f or >= 160.0f) 
        //     {
        //         return originalPower * 1.2f;
        //     }
        //
        //     return originalPower;
        // }
        //
        // // Fortitude +4
        // public float Fortitude4Set(float criticalChance, ICombatProvider provider, ICombatTaker taker)
        // {
        //     var providerForward = provider.gameObject.transform.forward;
        //     var takerForward    = taker.gameObject.transform.forward;
        //     var angle           = Vector3.Angle(providerForward, takerForward);
        //
        //     if (angle is <= 20.0f or >= 160.0f)
        //     {
        //         return criticalChance + 0.05f;
        //     }
        //
        //     return criticalChance;
        // }
        //
        // // Valor +2
        // public float Valor2Set(float original, CharacterBehaviour adventurer)
        // {
        //     var maxHp   = adventurer.StatTable.Health          * 10f;
        //     var hpRatio = adventurer.DynamicStatEntry.Hp.Value / maxHp;
        //
        //     if (hpRatio is < 0.8f and > 0.2f) return original;
        //     
        //     var allSkill = adventurer.SkillBehaviour.SkillList;
        //     
        //     allSkill.ForEach(skill =>
        //     {
        //         
        //     });
        //
        //     return original;
        // }
    }
}
