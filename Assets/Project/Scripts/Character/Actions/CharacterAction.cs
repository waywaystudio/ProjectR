using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Actions
{
    public class CharacterAction : MonoBehaviour, IEditable
    {
        /* Field */
        // [SerializeField] private SkillAction skillAction;
        
        /* Properties */
        /* Use In BehaviorTree. */
        
        [SerializeField] private GlobalCoolDown gcd;
        [SerializeField] private CommonAction commonAction;
        [SerializeField] private List<SkillComponent> skillList = new();
        [SerializeField] private SkillComponent firstSkill;
        [SerializeField] private SkillComponent secondSkill;
        [SerializeField] private SkillComponent thirdSkill;
        [SerializeField] private SkillComponent fourthSkill;

        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;
        public SkillComponent Current { get; set; }
        public List<SkillComponent> SkillList => skillList;
        public SkillComponent FirstSkill => firstSkill;
        public SkillComponent SecondSkill => secondSkill;
        public SkillComponent ThirdSkill => thirdSkill;
        public SkillComponent FourthSkill => fourthSkill;

        public ConditionTable GlobalConditions { get; } = new();
        public ActionTable<SkillComponent> OnGlobalActivated { get; } = new();
        public ActionTable<SkillComponent> OnGlobalCanceled { get; } = new();
        public ActionTable<SkillComponent> OnGlobalReleased { get; } = new();
        

        public void Run(Vector3 destination)
        {
            if (!Current.IsNullOrEmpty() && !Current.IsEnded && Current.IsRigid) return;
            
            CancelSkill();
            
            commonAction.Run(destination);
        }

        public void Rotate(Vector3 lookTargetPosition)
        {
            commonAction.Rotate(lookTargetPosition);
        }

        public void Stop()
        {
            commonAction.Stop();
        }

        public void Dash(Vector3 position, float distance)
        {
            CancelSkill();

            commonAction.Dash(position, distance);
        }

        public void Teleport(Vector3 direction, float distance)
        {
            CancelSkill();
            
            commonAction.Teleport(direction, distance);
        }

        public void Dead()
        {
            CancelSkill();
            
            commonAction.Dead();
        }
        

        public void ActiveSkill(SkillComponent skill, Vector3 targetPosition)
        {
            if (GlobalConditions.HasFalse) return;
            if (skill.ConditionTable.HasFalse) return;

            Rotate(targetPosition);
            
            Current = skill;
            Current.Activate();
            OnGlobalActivated.Invoke(skill);
        }

        public void CancelSkill()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Cancel();
            OnGlobalCanceled.Invoke(Current);
        }

        public void ReleaseSkill()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Release();
            OnGlobalReleased.Invoke(Current);
        }

        public bool TryGetMostPrioritySkill(out SkillComponent skill)
        {
            SkillComponent result = null;
            
            foreach (var skillComponent in skillList)
            {
                if (skillComponent.ConditionTable.HasFalse) continue;
                if (result is null || result.Priority < skillComponent.Priority)
                {
                    result = skillComponent;
                }
            }

            skill = result;
            return skill is not null;
        }


        private void Awake()
        {
            commonAction ??= GetComponentInChildren<CommonAction>();
            gcd          ??= GetComponent<GlobalCoolDown>();
            
            skillList.Clear();
            GetComponentsInChildren(false, skillList);
            skillList.ForEach(x => x.OnEnded.Register("BehaviourUnregister", () => Current = null));

            GlobalConditions.Register("GlobalCoolTime", () => !gcd.IsCooling);
            GlobalConditions.Register("CurrentSkillCompleted", () => IsSkillEnded);
            
            OnGlobalActivated.Register("GlobalCooling", gcd.StartCooling);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            TryGetComponent(out gcd);
            TryGetComponent(out commonAction);
            
            skillList.Clear();
            GetComponentsInChildren(false, skillList);
        }
#endif
    }
}
