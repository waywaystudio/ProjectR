using System.Collections.Generic;
using Common.Skills;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class SkillBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private CoolTimer globalCoolTimer;
        [SerializeField] private Table<DataIndex, SkillComponent> skillTable;
        
        private CharacterBehaviour cb;

        public List<DataIndex> SkillIndexList => skillTable.KeyList;
        public ActionMask BehaviourMask =>Current ? Current.BehaviourMask : ActionMask.Skill;
        public Sequencer Sequence { get; } = new();
        public SequenceBuilder Builder { get; private set; }
        public SequenceInvoker Invoker { get; private set; }
        
        public bool IsGlobalCoolTimeReady => globalCoolTimer.IsReady;
        public bool IsSkillEnded => Current.IsNullOrEmpty() || Current.IsEnded;
        public SkillComponent Current { get; set; }
        public ActionTable OnSkillChanged { get; } = new();


        public void Initialize(CharacterBehaviour character)
        {
            cb = character;
            
            Invoker = new SequenceInvoker(Sequence);
            Builder = new SequenceBuilder(Sequence);
            Builder
                .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(cb.BehaviourMask))
                .AddCondition("IsSkillEnded", () => IsSkillEnded)
                .AddCondition("GlobalCoolTimeReady", () => globalCoolTimer.IsReady)
                .Add(Section.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                .Add(Section.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                .Add(Section.Active,"PlayGlobalCoolTimer", globalCoolTimer.Play)
                .Add(Section.Cancel,"CurrentSkillCancel", () => Current.Invoker.Cancel())
                .Add(Section.End,"Stop", cb.Stop);
            
            globalCoolTimer.SetRetriever(() => cb.StatTable.Haste);
            skillTable.Iterate(skill =>
            {
                skill.Initialize();
                skill.Builder
                     .Add(Section.End, "BehaviourUnregister", () => Current = null)
                     .Add(Section.End, "SkillBehaviourEnd", Invoker.End);
            });
        }
        


        public void Active(DataIndex actionCode, Vector3 targetPosition)
        {
            if (!Invoker.IsAbleToActive) return;

            var skill = skillTable[actionCode];

            if (!skill.Invoker.IsAbleToActive) return;
            
            Current = skill;
            Invoker.Active();
            cb.Rotate(targetPosition);
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
        
        public SkillComponent GetSkill(DataIndex actionCode) => skillTable[actionCode];

        public bool TryGetMostPrioritySkill(out SkillComponent skill)
        {
            SkillComponent result = null;
            
            skillTable.Iterate(skill =>
            {
                if (skill.Invoker is null) return;
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

        public void OnFocusVenturerChanged(CharacterBehaviour cb)
        {
            var isFocusTarget = this.cb == cb;
            
            skillTable.Iterate(skill => skill.ActiveEffect(isFocusTarget));
        }

        public void Dispose()
        {
            skillTable.Iterate(skill => skill.Dispose());

            Sequence.Clear();
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
