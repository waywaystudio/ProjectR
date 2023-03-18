using System.Collections;
using System.Collections.Generic;
using Common.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class SkillBehaviour : ActionBehaviour, IEditable
    {
        [ShowInInspector] 
        private const float GlobalCoolTime = 1.2f;
        
        [SerializeField] private List<SkillSequence> skillList = new();
        
        private Coroutine gcdRoutine;

        public override CharacterActionMask BehaviourMask => CharacterActionMask.Skill;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.None |
                                                             CharacterActionMask.Stop |
                                                             CharacterActionMask.Run;
        
        public ActionTable<SkillSequence> OnExecuting { get; } = new();
        public ActionTable OnReleased { get; } = new();

        public SkillSequence Current { get; set; }
        public FloatEvent GlobalCoolTimeProgress { get; } = new(0, float.MaxValue);
        public List<SkillSequence> SkillList => skillList;

        [ShowInInspector]
        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;
        public bool IsGlobalCooling => GlobalCoolTimeProgress.Value != 0f;


        public SkillSequence GetSkill(DataIndex actionCode)
        {
            return skillList.Find(item => item.ActionCode == actionCode);
        }
        
        public bool IsAble(SkillSequence skill) => Conditions.IsAllTrue && skill.Conditions.IsAllTrue;
        
        public void Active(DataIndex actionCode, Vector3 targetPosition)
        {
            var skill = skillList.Find(item => item.ActionCode == actionCode);

            if (skill.IsNullOrEmpty())
            {
                Debug.LogError($"Can't Find {actionCode} skill in {Cb.Name}'s SkillList");
                return;
            }

            Active(skill, targetPosition);
        }

        public void Active(SkillSequence skill, Vector3 targetPosition)
        {
            if (!IsAble(skill)) return;
            
            RegisterBehaviour(Cb);
            
            Current = skill;
            Current.Execution(targetPosition);

            OnActivated.Invoke();
            OnExecuting.Invoke(skill);
        }

        public void Cancel()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Cancellation();
        }

        public void Release()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Release();
            OnReleased.Invoke();
        }

        public bool TryGetMostPrioritySkill(out SkillSequence skill)
        {
            SkillSequence result = null;
            
            foreach (var skillComponent in SkillList)
            {
                if (skillComponent.Conditions.HasFalse) continue;
                if (result is null || result.Priority < skillComponent.Priority)
                {
                    result = skillComponent;
                }
            }

            skill = result;
            return skill is not null;
        }


        private void StartCooling()
        {
            if (gcdRoutine != null) StopCoroutine(gcdRoutine);
            gcdRoutine = StartCoroutine(Cooling());
        }
        
        private IEnumerator Cooling()
        {
            var hastedCoolTime = GlobalCoolTime * CharacterUtility.GetHasteValue(Cb.StatTable.Haste);
            
            GlobalCoolTimeProgress.Value = hastedCoolTime;

            while (GlobalCoolTimeProgress.Value > 0)
            {
                GlobalCoolTimeProgress.Value -= Time.deltaTime;
                yield return null;
            }
        }

        private void OnEnable()
        {
            skillList.ForEach(skill => skill.OnEnded.Register("BehaviourUnregister", () => Current = null));
            
            Conditions.Register("IsOverBehavior", IsOverBehaviour);
            Conditions.Register("GlobalCoolTime", () => !IsGlobalCooling);
            Conditions.Register("CurrentSkillCompleted", () => IsSkillEnded);
            
            OnCanceled.Register("CancelAction", Cancel);
            
            OnActivated.Register("GlobalCooling", StartCooling);
        }

        private void OnDisable()
        {
            skillList.ForEach(skill => skill.OnEnded.Unregister("BehaviourUnregister"));
            
            Conditions.Unregister("IsOverBehavior");
            Conditions.Unregister("GlobalCoolTime");
            Conditions.Unregister("CurrentSkillCompleted");
            
            OnCanceled.Unregister("CancelAction");
            
            OnActivated.Unregister("GlobalCooling");
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(false, skillList);
        }
#endif
    }
}
