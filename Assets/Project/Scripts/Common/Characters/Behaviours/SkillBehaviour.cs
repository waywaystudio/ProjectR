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
        
        [SerializeField] private List<SkillComponent> skillList = new();
        
        private Coroutine gcdRoutine;

        public override CharacterActionMask BehaviourMask => CharacterActionMask.Skill;
        public override CharacterActionMask IgnorableMask => CharacterActionMask.None |
                                                             CharacterActionMask.Stop |
                                                             CharacterActionMask.Run;
        
        public ActionTable<SkillComponent> OnExecuting { get; } = new();
        public ActionTable OnReleased { get; } = new();

        public SkillComponent Current { get; set; }
        public FloatEvent GlobalCoolTimeProgress { get; } = new(0, float.MaxValue);
        public List<SkillComponent> SkillList => skillList;
        
        public SkillComponent FirstSkill => SkillList[0];
        public SkillComponent SecondSkill => SkillList[1];
        public SkillComponent ThirdSkill => SkillList[2];
        public SkillComponent FourthSkill => SkillList[3];
        
        [ShowInInspector]
        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;
        public bool IsGlobalCooling => GlobalCoolTimeProgress.Value != 0f;


        public SkillComponent GetSkill(DataIndex actionCode)
        {
            return skillList.Find(item => item.ActionCode == actionCode);
        }
        
        public bool IsAble(SkillComponent skill) => Conditions.IsAllTrue && skill.Conditions.IsAllTrue;
        
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

        public void Active(SkillComponent skill, Vector3 targetPosition)
        {
            if (!IsAble(skill)) return;
            
            RegisterBehaviour(Cb);
            
            Current = skill;
            Current.Activate(targetPosition);

            OnActivated.Invoke();
            OnExecuting.Invoke(skill);
        }

        public void Cancel()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Cancel();
            
            // TODO. 보통 Cancel()에서 OnCanceled를 호출하는데, OnCanceled에 Cancel()을 Register 하고 있다. 요상하다.
            // OnCanceled.Invoke();
        }

        public void Release()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Release();
            OnReleased.Invoke();
        }

        public bool TryGetMostPrioritySkill(out SkillComponent skill)
        {
            SkillComponent result = null;
            
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
