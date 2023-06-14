using System.Collections.Generic;
using Common.Skills;
using Sequences;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class SkillBehaviour : MonoBehaviour, IActionBehaviour, IEditable
    {
        [SerializeField] private AwaitTimer globalCoolTimer = new();
        
        // SerializationData로 부터, 캐릭터 혹은 몬스터의 스킬을 받아와야 한다.
        // 캐릭터는 스킬이 변경되었을 가능성이 있고, 보스나 미니언은 사실상 거의 없다고 본다.
        // SerializeField는 유지할 예정이며, 캐릭터의 경우만 Load하는 함수를 짜서 변경할 수 있으니,
        // 일단은 현재 상태 안에서 구현
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
            globalCoolTimer.Timer = 1.2f;
            globalCoolTimer.Action += () =>
            {
                Debug.Log($"Current frame:{Time.frameCount}. Name:{name}. Instance:{GetInstanceID()}");
            };
            
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
