using System.Collections.Generic;
using Common.Skills;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class SkillBehaviour : ActionBehaviour, IEditable
    {
        [SerializeField] private List<SkillComponent> skillList = new();

        public override CharacterActionMask BehaviourMask =>
            Current ? Current.BehaviourMask : CharacterActionMask.Skill;
        public override CharacterActionMask IgnorableMask => 
            Current ? Current.IgnorableMask : CharacterActionMask.SkillIgnoreMask;

        public SkillComponent Current { get; set; }
        public ActionTable OnReleased { get; } = new();
        public List<SkillComponent> SkillList => skillList;
        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;
        

        public SkillComponent GetSkill(DataIndex actionCode)
        {
            return skillList.Find(item => item.DataIndex == actionCode);
        }

        public void Active(DataIndex actionCode, Vector3 targetPosition)
        {
            SkillComponent skill = null;
            
            foreach (var item in skillList)
            {
                if (item.DataIndex != actionCode) continue;
                
                skill = item;
                break;
            }

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
        }

        public override void Cancel()
        {
            if (Current.IsNullOrEmpty()) return;
            
            OnCanceled.Invoke();
            Current.Cancel();
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
        

        private bool IsAble(IConditionalSequence skill)
        {
            return CanOverrideToCurrent
                   && IsSkillEnded 
                   && Conditions.IsAllTrue 
                   && skill.Conditions.IsAllTrue;
        }
        
        private void OnEnable()
        {
            skillList.ForEach(skill => skill.OnEnded.Register("BehaviourUnregister", () => Current = null));
        }

        private void OnDisable() => Dispose();


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(false, skillList);
        }
#endif
    }
}
