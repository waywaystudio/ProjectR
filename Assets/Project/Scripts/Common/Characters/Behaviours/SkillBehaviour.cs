using System;
using System.Collections.Generic;
using Common.Skills;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class SkillBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private AwaitTimer globalCoolTimer;
        [SerializeField] private Table<DataIndex, SkillComponent> skillTable;
        [SerializeField] private Sequencer sequencer;

        public List<DataIndex> SkillIndexList => skillTable.KeyList;
        public ActionMask BehaviourMask =>Current ? Current.BehaviourMask : ActionMask.Skill;
        public SequenceBuilder SequenceBuilder { get; } = new();
        public SequenceInvoker SequenceInvoker { get; } = new();
        
        public bool IsGlobalCoolTimeReady => globalCoolTimer.IsReady;
        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;
        public SkillComponent Current { get; set; }
        
        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public SkillComponent GetSkill(DataIndex actionCode) => skillTable[actionCode];


        public void Active(DataIndex actionCode, Vector3 targetPosition)
        {
            if (!SequenceInvoker.IsAbleToActive) return;

            var skill = skillTable[actionCode];

            if (!skill.SkillInvoker.IsAbleToActive) return;
            
            Current = skill;
            SequenceInvoker.Active();
            Current.SkillInvoker.Active(targetPosition);
        }

        public void Cancel()
        {
            if (Current.IsNullOrDestroyed()) return;

            Current.SkillInvoker.Cancel();
        }

        public void Release()
        {
            if (Current.IsNullOrDestroyed()) return;

            Current.SkillInvoker.Release();
        }

        public bool TryGetMostPrioritySkill(out SkillComponent skill)
        {
            SkillComponent result = null;
            
            skillTable.Iterate(skill =>
            {
                if (!skill.SkillInvoker.IsAbleToActive) return;
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
            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer)
                           .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                           .AddCondition("IsSkillEnded", () => IsSkillEnded)
                           .AddCondition("GlobalCoolTimeReady", () => globalCoolTimer.IsReady)
                           .Add(SectionType.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                           .Add(SectionType.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                           .Add(SectionType.Active,"PlayGlobalCoolTimer", globalCoolTimer.Play)
                           .Add(SectionType.Cancel,"CurrentSkillCancel", () => Current.SkillInvoker.Cancel())
                           .Add(SectionType.End,"Stop", Cb.Stop);
            
            skillTable.Iterate(skill =>
            {
                skill.Initialize();
                skill.SequenceBuilder
                     .Add(SectionType.End, "BehaviourUnregister", () => Current = null);
            });
        }

        private void OnDisable()
        {
            skillTable.Iterate(skill => { skill.Dispose(); });

            sequencer.Clear();
            globalCoolTimer.Dispose();
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
