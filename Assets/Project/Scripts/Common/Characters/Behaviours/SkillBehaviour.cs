using System.Collections.Generic;
using Common.Skills;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class SkillBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private AwaitTimer globalCoolTimer;
        [SerializeField] private Table<DataIndex, SkillComponent> skillTable = new();
        [SerializeField] private Sequencer sequencer;

        public List<DataIndex> SkillIndexList => skillTable.KeyList;
        public CharacterActionMask BehaviourMask =>Current ? Current.BehaviourMask : CharacterActionMask.Skill;
        public CharacterActionMask IgnorableMask =>Current ? Current.IgnorableMask : CharacterActionMask.SkillIgnoreMask;
        public Sequencer Sequencer => sequencer;

        public bool IsGlobalCoolTimeReady => globalCoolTimer.IsReady;
        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;
        

        public SkillComponent Current { get; set; }

        private bool CanOverrideToCurrent => (IgnorableMask | Cb.BehaviourMask) == IgnorableMask;
        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public SkillComponent GetSkill(DataIndex actionCode) => skillTable[actionCode];

        public void Active(DataIndex actionCode, Vector3 targetPosition)
        {
            if (!sequencer.IsAbleToActive) return;
            if (!skillTable.TryGetValue(actionCode, out var skill))
            {
                Debug.Log($"Input :{actionCode}. TableCount:{skillTable.Count}");
                return;
            }

            if (!skill.Sequencer.IsAbleToActive) return;
            
            sequencer.Active();
            
            Current = skill;
            Current.Activate(targetPosition);
        }

        public void Cancel()
        {
            if (Current.IsNullOrEmpty()) return;
            
            sequencer.Cancel();
        }
        
        public void Release()
        {
            if (Current.IsNullOrEmpty()) return;
            
            Current.Release();
        }

        public bool TryGetMostPrioritySkill(out SkillComponent skill)
        {
            SkillComponent result = null;
            
            skillTable.Iterate(skill =>
            {
                if (!skill.Sequencer.IsAbleToActive) return;
                if (result is null || result.Priority < skill.Priority)
                {
                    result = skill;
                }
            });

            skill = result;
            return skill is not null;
        }
        

        private void OnEnable()
        {
            skillTable.Iterate(skill => skill.Sequencer.EndAction.Add("BehaviourUnregister", () => Current = null));
            
            sequencer.Condition.Add("CanOverrideToCurrent", () => CanOverrideToCurrent);
            sequencer.Condition.Add("IsSkillEnded", () => IsSkillEnded);
            sequencer.Condition.Add("GlobalCoolTimeReady", () => globalCoolTimer.IsReady);

            sequencer.ActiveAction.Add("CommonStunAction", () =>
            {
                if (cb.CurrentBehaviour is not null && cb.BehaviourMask != BehaviourMask)
                    cb.CurrentBehaviour.Cancel();

                cb.CurrentBehaviour = this;
                globalCoolTimer.Play();
            });

            sequencer.CancelAction.Add("CurrentSkillCancel", () => Current.Cancel());
            sequencer.EndAction.Add("Stop", Cb.Stop);
        }

        private void OnDisable()
        {
            sequencer.Clear();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            var preLoadedSkillList = GetComponentsInChildren<SkillComponent>();
            
            skillTable.CreateTable(preLoadedSkillList, skill => skill.DataIndex);
        }
#endif
    }
}
