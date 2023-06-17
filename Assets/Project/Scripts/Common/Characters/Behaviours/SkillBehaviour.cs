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
            if (!skillTable.TryGetValue(actionCode, out var skill))
            {
                Debug.Log($"Input :{actionCode}. TableCount:{skillTable.Count}");
                return;
            }

            if (!skill.SkillSequencer.IsAbleToActive) return;
            
            Current = skill;
            
            SequenceInvoker.Active();
            Current.Active(targetPosition);
        }

        public void Cancel()
        {
            if (Current.IsNullOrEmpty()) return;
            
            SequenceInvoker.Cancel();
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
                if (!skill.SkillSequencer.IsAbleToActive) return;
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
            skillTable.Iterate(skill => skill.SkillSequencer.EndAction.Add("BehaviourUnregister", () => Current = null));
            
            SequenceInvoker.Initialize(sequencer);
            SequenceBuilder.Initialize(sequencer)
                           .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                           .AddCondition("IsSkillEnded", () => IsSkillEnded)
                           .AddCondition("GlobalCoolTimeReady", () => globalCoolTimer.IsReady)
                           .AddActive("CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToCancel(this))
                           .AddActive("SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                           .AddActive("PlayGlobalCoolTimer", globalCoolTimer.Play)
                           .AddCancel("CurrentSkillCancel", () => Current.Cancel())
                           .AddEnd("Stop", Cb.Stop);
        }

        private void OnDisable()
        {
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
