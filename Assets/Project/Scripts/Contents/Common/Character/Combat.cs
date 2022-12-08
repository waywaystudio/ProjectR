using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Character.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Character
{
    using Skills.Core;

    public class Combat : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private CommonAttack commonAttack;
        [SerializeField] private AimShot aimShot;

        private bool isGlobalCooling;
        private float globalCoolTime = 1.2f;
        private BaseSkill nextSkill;
        
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        public Dictionary<int, BaseSkill> SkillTable { get; } = new();
        public bool IsReadyToAnySkill => !isGlobalCooling && SkillTable.Any(x => x.Value.IsCoolTimeReady);
        
        // TEST
        [Button] private void ActiveCommonAttack() => ActiveSkill(commonAttack);
        [Button] private void ActiveAimShot() => ActiveSkill(aimShot);
        //

        private void ActiveSkill(BaseSkill skill)
        {
            if (!IsReadyToAnySkill)
            {
                Debug.Log("Any of Skill is Not Ready");
                return;
            }
            
            if (!skill.IsSkillReady)
            {
                Debug.Log($"{skill.SkillName}'s Some Entity is Not ready...");
                return;
            }
            
            if (nextSkill != null) UnregisterSkill(nextSkill);

            RegisterSkill(skill);
            ActiveNextSkill(skill.AnimationKey);
            StartCoroutine(GlobalCoolDownRoutine());
        }


        public bool TryRegisterSkill(out BaseSkill skill)
        {
            if (isGlobalCooling)
            {
                skill = null;
                return false;
            }
            
            if (!TryGetReadySkill(out skill)) return false;
            if (nextSkill != null) UnregisterSkill(nextSkill);

            RegisterSkill(skill);
            ActiveNextSkill(skill.AnimationKey);
            StartCoroutine(GlobalCoolDownRoutine());

            return true;
        }

        private void ActiveNextSkill(string animationKey)
        {
            switch (animationKey)
            {
                case "Attack" : Cb.Attack();
                    break;
                case "Skill" : Cb.Skill();
                    break;
                // add more case according to Animation, DamageMechanic...
            }
        }
        
        private bool TryGetReadySkill(out BaseSkill skill)
        {
            for (var i = 0; i < SkillTable.Count; ++i)
            {
                if (!SkillTable[i].IsCoolTimeReady) continue;
                
                if (nextSkill != null) nextSkill.UnRegister();
                
                skill = SkillTable[i];
                return true;
            }

            skill = null;
            return false;
        }
        
        private void RegisterSkill(BaseSkill skill)
        {
            nextSkill = skill;
            skill.Register();
            
            if (skill.AnimationKey == "Attack")
            {
                Cb.OnAttack += nextSkill.StartSkill;
                Cb.OnAttackHit += nextSkill.InvokeEvent;
            }
            else if (skill.AnimationKey == "Skill")
            {
                Cb.OnSkill += nextSkill.StartSkill;
                Cb.OnSkillHit += nextSkill.InvokeEvent;
            }
        }

        private void UnregisterSkill(BaseSkill skill)
        {
            skill.UnRegister();

            if (skill.AnimationKey == "Attack")
            {
                Cb.OnAttack -= nextSkill.StartSkill;
                Cb.OnAttackHit -= nextSkill.InvokeEvent;
            }
            else if (skill.AnimationKey == "Skill")
            {
                Cb.OnSkill -= nextSkill.StartSkill;
                Cb.OnSkillHit -= nextSkill.InvokeEvent;
            }
        }

        private IEnumerator GlobalCoolDownRoutine()
        {
            globalCoolTime = 1.0f;
            isGlobalCooling = true;

            while (globalCoolTime > 0)
            {
                globalCoolTime -= Time.deltaTime;
                yield return null;
            }

            isGlobalCooling = false;
        }
    }
}
