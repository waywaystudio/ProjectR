using System.Collections.Generic;
using Common.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Actions
{
    public class SkillAction : MonoBehaviour, ICharacterAction, IEditable
    {
        [SerializeField] private GlobalCoolDown gcd;
        [SerializeField] private List<SkillComponent> skillList = new();

        public CharacterActionMask ActionType => CharacterActionMask.Skill;
        public CharacterActionMask DisableActionMask => CharacterActionMask.None;
        
        [ShowInInspector]
        public ConditionTable Conditions { get; } = new();
        public ActionTable<SkillComponent> OnActivated { get; } = new();
        public ActionTable<SkillComponent> OnCanceled { get; } = new();
        public ActionTable<SkillComponent> OnReleased { get; } = new();
        public SkillComponent Current { get; set; }
        public List<SkillComponent> SkillList => skillList;
        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;
        public bool IsCooling => gcd.IsCooling;


        public bool IsAble(SkillComponent skill) => Conditions.IsAllTrue && skill.Conditions.IsAllTrue;
        
        public void ActiveSkill(SkillComponent skill, Vector3 position)
        {
            Current = skill;
            Current.Activate(position);
            OnActivated.Invoke(skill);
        }

        public void CancelSkill()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Cancel();
            OnCanceled.Invoke(Current);
        }

        public void ReleaseSkill()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Release();
            OnReleased.Invoke(Current);
        }
        
        
        private void Awake()
        {
            gcd ??= GetComponent<GlobalCoolDown>();
            
            GetComponentsInChildren(false, skillList);
            skillList.ForEach(x => x.OnEnded.Register("BehaviourUnregister", () => Current = null));

            Conditions.Register("GlobalCoolTime", () => !gcd.IsCooling);
            Conditions.Register("CurrentSkillCompleted", () => IsSkillEnded);
            
            OnActivated.Register("GlobalCooling", gcd.StartCooling);
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
