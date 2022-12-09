using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character.Operation.Combating
{
    using Skills;
    
    public class Combat : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private CommonAttack commonAttack;
        [SerializeField] private AimShot aimShot;

        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        [ShowInInspector]
        public Dictionary<int, BaseSkill> SkillTable { get; } = new();
        public BaseSkill CurrentSkill { get; set; }
        public float GlobalCoolTime { get; set; } = 1.2f;
        public bool IsGlobalCooling { get; set; }
        public bool IsCoolOnAnySkill => SkillTable.Any(x => x.Value.IsCoolTimeReady);

        public bool TryGetMostPrioritySkill(out BaseSkill mostPrioritySkill)
        {
            mostPrioritySkill = SkillTable
                .Where(x => x.Value.IsCoolTimeReady)
                .MaxBy(x => x.Value.Priority).Value;
            
            Debug.Log($"count : {SkillTable.Count} : {mostPrioritySkill.SkillName}");

            return mostPrioritySkill != null;
        }
        
        // TEST
        [Button] private void ActiveCommonAttack() => UseSkill(commonAttack);
        [Button] private void ActiveAimShot() => UseSkill(aimShot);
        //

        public void UseSkill(BaseSkill skill)
        {
            // 아래 조건은 BD에서 처리할 예정
            if (IsGlobalCooling || !IsCoolOnAnySkill || !skill.IsSkillReady)
            {
                Debug.Log($"Skill is not Ready. Global Cool? : {IsGlobalCooling}, IsAny Skill Ready? : {IsCoolOnAnySkill}, Specific Skill Ready ? : {skill.IsSkillReady}");
                return;
            }

            if (CurrentSkill != null) DeActiveSkill(CurrentSkill);

            ActiveSkill(skill);
            GlobalCoolDownOn();
        }

        private void ActiveSkill(BaseSkill skill)
        {
            CurrentSkill = skill;
            skill.OnActiveSkill();
        }

        private void DeActiveSkill(BaseSkill skill)
        {
            CurrentSkill = null;
            skill.DeActiveSkill();
        }

        private void GlobalCoolDownOn() => StartCoroutine(GlobalCoolDownRoutine(GlobalCoolTime));
        private IEnumerator GlobalCoolDownRoutine(float coolTime)
        {
            var coolTimer = coolTime;
            
            IsGlobalCooling = true;

            while (coolTimer > 0)
            {
                coolTimer -= Time.deltaTime;
                yield return null;
            }

            IsGlobalCooling = false;
        }
    }
}
