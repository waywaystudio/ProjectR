using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Actions
{
    public class SkillAction : MonoBehaviour, IEditable
    {
        [SerializeField] private GlobalCoolDown gcd;
        [SerializeField] private List<SkillComponent> skillList = new();
        
        public ConditionTable GlobalConditions { get; } = new();
        public ActionTable<SkillComponent> OnGlobalActivated { get; } = new();
        public ActionTable<SkillComponent> OnGlobalCanceled { get; } = new();
        public ActionTable<SkillComponent> OnGlobalReleased { get; } = new();
        public SkillComponent Current { get; set; }
        public List<SkillComponent> SkillList => skillList;
        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;


        public bool IsAble(SkillComponent skill) => GlobalConditions.IsAllTrue && skill.ConditionTable.IsAllTrue;
        
        public void ActiveSkill(SkillComponent skill)
        {
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
        
        
        private void Awake()
        {
            gcd ??= GetComponent<GlobalCoolDown>();
            
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
            GetComponentsInChildren(false, skillList);
        }
#endif
    }
}
