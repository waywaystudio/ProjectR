using System.Collections.Generic;
using Common.Skills;
using Sequences;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class SkillBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private AwaitTimer globalCoolTimer = new();
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
            
            sequencer.ActiveSequence();
            
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

        public void SkillBehaviourRegisterActive()
        {
            if (Cb.CurrentBehaviour is not null && Cb.BehaviourMask != BehaviourMask)
            {
                Cb.CurrentBehaviour.Cancel();
            }

            Cb.CurrentBehaviour = this;
        }
        
        public void SkillBehaviourGlobalCoolTimeActive()
        {
            globalCoolTimer.Play();
        }

        public void SkillBehaviourCancel()
        {
            Current.Cancel();
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


        private void Awake()
        {
            globalCoolTimer       = new AwaitTimer();
            globalCoolTimer.Timer = 1.5f;
            skillTable.Iterate(skill => skill.OnEnded.Register("BehaviourUnregister", () => Current = null));
            
            sequencer.Condition.Add("CanOverrideToCurrent", () => CanOverrideToCurrent);
            sequencer.Condition.Add("IsSkillEnded", () => IsSkillEnded);
            sequencer.Condition.Add("GlobalCoolTimeReady", () => globalCoolTimer.IsReady);
        }

        private void OnDestroy()
        {
            sequencer.Clear();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            var preLoadedSkillList = GetComponentsInChildren<SkillComponent>();
            
            skillTable.CreateTable(preLoadedSkillList, skill => skill.DataIndex);
            sequencer.AssignPersistantEvents();
        }
#endif
    }
}
