using System.Collections.Generic;
using Common.Skills;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class SkillBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private CoolTimer globalCoolTimer;
        [SerializeField] private Table<DataIndex, SkillComponent> skillTable;
        [SerializeField] private Sequencer sequencer;

        public List<DataIndex> SkillIndexList => skillTable.KeyList;
        public ActionMask BehaviourMask =>Current ? Current.BehaviourMask : ActionMask.Skill;
        public SequenceBuilder SequenceBuilder { get; private set; }
        public SequenceInvoker SequenceInvoker { get; private set; }
        
        public bool IsGlobalCoolTimeReady => globalCoolTimer.IsReady;
        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;
        public SkillComponent Current { get; set; }
        public ActionTable OnSkillChanged { get; } = new();

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        public SkillComponent GetSkill(DataIndex actionCode) => skillTable[actionCode];


        public void Active(DataIndex actionCode, Vector3 targetPosition)
        {
            if (!SequenceInvoker.IsAbleToActive) return;

            var skill = skillTable[actionCode];

            if (!skill.Invoker.IsAbleToActive) return;
            
            Current = skill;
            SequenceInvoker.Active();
            Cb.Rotate(targetPosition);
            Current.Invoker.Active(targetPosition);
        }

        public void Cancel()
        {
            if (Current.IsNullOrDestroyed()) return;

            Current.Invoker.Cancel();
        }

        public void Release()
        {
            if (Current.IsNullOrDestroyed()) return;

            Current.Invoker.Release();
        }

        public bool TryGetMostPrioritySkill(out SkillComponent skill)
        {
            SkillComponent result = null;
            
            skillTable.Iterate(skill =>
            {
                if (!skill.Invoker.IsAbleToActive) return;
                if (result is null || result.Priority < skill.Priority)
                {
                    result = skill;
                }
            });

            skill = result;
            return skill is not null;
        }

        public void ChangeSkill(DataIndex originSkill, DataIndex toSkill)
        {
            skillTable.SwapOrder(originSkill, toSkill);
            OnSkillChanged.Invoke();
        }
        
        
        private void OnEnable()
        {
            SequenceInvoker = new SequenceInvoker(sequencer);
            SequenceBuilder = new SequenceBuilder(sequencer);
            SequenceBuilder.AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                           .AddCondition("IsSkillEnded", () => IsSkillEnded)
                           .AddCondition("GlobalCoolTimeReady", () => globalCoolTimer.IsReady)
                           .Add(Section.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                           .Add(Section.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                           .Add(Section.Active,"PlayGlobalCoolTimer", globalCoolTimer.Play)
                           .Add(Section.Cancel,"CurrentSkillCancel", () => Current.Invoker.Cancel())
                           .Add(Section.End,"Stop", Cb.Stop);
            
            globalCoolTimer.SetRetriever(() => Cb.StatTable.Haste);
            skillTable.Iterate(skill =>
            {
                skill.Initialize();
                skill.Builder
                     .Add(Section.End, "BehaviourUnregister", () => Current = null)
                     .Add(Section.End, "SkillBehaviourEnd", SequenceInvoker.End);
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
            var preLoadedSkillList = new List<SkillComponent>();
            
            GetComponentsInChildren(preLoadedSkillList);

            skillTable.CreateTable(preLoadedSkillList, skill => skill.DataIndex);
        }
#endif
    }
}
