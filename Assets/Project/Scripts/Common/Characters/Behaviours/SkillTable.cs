using System.Collections.Generic;
using Common.Skills;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class SkillTable : MonoBehaviour, IEditable
    {
        [SerializeField] private Table<DataIndex, SkillComponent> table;

        public CharacterBehaviour Cb { get; private set; }
        public List<DataIndex> SkillIndexList => table.KeyList;
        public ActionTable<SkillTable> OnSkillTableChanged { get; } = new();
        public SkillComponent Current { get; set; }
        public SkillComponent this[DataIndex key] => table[key];


        public void Initialize(CharacterBehaviour character)
        {
            Cb = character;

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
                Debug.LogWarning($"Not Exist {actionCode} in {Cb.DataIndex}");
                return;
            }

            if (!skill.Invoker.IsAbleToActive) return;
            
            Cb.Rotate(targetPosition);
            
            Current = skill;
            Current.Invoker.Active(targetPosition);
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
            OnSkillTableChanged.Invoke(this);
        }

        public void OnFocusVenturerChanged(CharacterBehaviour cb)
        {
            var isFocusTarget = 
                Cb == cb;
            
            table.Iterate(skill => skill.ActiveEffect(isFocusTarget));
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
