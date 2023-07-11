using System.Collections.Generic;
using Common.Skills;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class SkillTable : MonoBehaviour, IEditable
    {
        [SerializeField] private Table<DataIndex, SkillComponent> table;
        
        private CharacterBehaviour cb;

        public List<DataIndex> SkillIndexList => table.KeyList;
        public ActionTable OnSkillSetChanged { get; } = new();
        public SkillComponent Current { get; set; }
        public SkillComponent this[DataIndex key] => table[key];


        public void Initialize(CharacterBehaviour character)
        {
            cb = character;

            table.Iterate(skill =>
            {
                skill.Initialize();
                skill.Builder
                     .Add(Section.End, "BehaviourUnregister", () => Current = null);
            });
        }

        public void Active(DataIndex actionCode, Vector3 targetPosition)
        {
            if (!table.TryGetValue(actionCode, out var skill))
            {
                Debug.LogWarning($"Not Exist {actionCode} in {cb.DataIndex}");
                return;
            }

            if (!skill.Invoker.IsAbleToActive) return;
            
            cb.Rotate(targetPosition);
            
            Current = skill;
            Current.Invoker.Active(targetPosition);
        }

        public void Release()
        {
            if (Current.IsNullOrDestroyed()) return;

            Current.Invoker.Release();
        }

        public bool TryGetMostPrioritySkill(out SkillComponent skill)
        {
            SkillComponent result = null;
            
            table.Iterate(skill =>
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
            table.SwapOrder(originSkill, toSkill);
            OnSkillSetChanged.Invoke();
        }

        public void OnFocusVenturerChanged(CharacterBehaviour cb)
        {
            var isFocusTarget = this.cb == cb;
            
            table.Iterate(skill => skill.ActiveEffect(isFocusTarget));
        }

        public void Dispose()
        {
            table.Iterate(skill => skill.Dispose());
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            var preLoadedSkillList = new List<SkillComponent>();
            
            GetComponentsInChildren(preLoadedSkillList);

            table.CreateTable(preLoadedSkillList, skill => skill.DataIndex);
        }
#endif
    }
}
