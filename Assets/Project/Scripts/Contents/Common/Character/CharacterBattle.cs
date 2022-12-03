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
