using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Common.Character
{
    using Skills;
    using Skills.Core;
    using Skills.Entity;
    
    /*
     * CharacterBattle -> 'Class'Skill : SkillOperation
     * CharacterBehavior를 Variant로 만들어야 하는 필요조건 파트.
     * SkillOperation 과 직업Skill 클래스로 구분 후 각 구현.
     * 먼저 직업Skill 클래스를 작성해보고, 공통으로 만들 수 있는 것은 빼자.
     */
    public class CharacterBattle : MonoBehaviour
    {
        [SerializeField] private CommonAttack commonAttack;
        [SerializeField] private AimShot aimShot;

        public CommonAttack CommonAttack => commonAttack;
        public AimShot AimShot => aimShot;

        // public void CommonAttack(ICombatAttribution combatAttribution, List<ICombatTaker> targetList)
        // {
            // commonAttack.SetEntities(combatAttribution);
            
            // 근데 결국에 데미지 주는 것도, 함수에서 이렇게 바로 실행하는게 아니라 애니매이션 이벤트에 달려있어야 할텐데;
            // commonAttack.TargetList.ForEach(x => x.TakeDamage(commonAttack));
            // targetList.ForEach(x => x.TakeDamage(commonAttack));
        // }

        // public void AimShot(ICombatAttribution combatAttribution, List<ICombatTaker> targetList)
        // {
        //     aimShot.SetEntities(combatAttribution);
        //     targetList.ForEach(x => x.TakeDamage(commonAttack));
        // }

        // public bool TrySetEntity<T>(string skillName, ISkillEntity skillEntity, out T skill) where T : SkillAttribution
        // {
        //     if (!SkillTable.TryGetValue(skillName, out var result))
        //     {
        //         Debug.LogWarning($"Not Exist {skillName}");
        //         return false;
        //     }
        //     
        //     result.SetEntities(skillEntity);
        //     return true;
        // }

        protected void Awake()
        {
            // SkillList = GetComponentsInChildren<SkillAttribution>().ToList();
            // SkillList.ForEach(x => SkillTable.Add(x.SkillName, x));
        }

        protected void Update()
        {
            if (commonAttack.isActiveAndEnabled) commonAttack.UpdateStatus();
            if (aimShot.isActiveAndEnabled) aimShot.UpdateStatus();
        }
    }
}
