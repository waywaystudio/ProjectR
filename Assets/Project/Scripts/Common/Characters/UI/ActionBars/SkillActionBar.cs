using System.Collections.Generic;
using UnityEngine;

namespace Common.Characters.UI.ActionBars
{
    public class SkillActionBar : MonoBehaviour, IEditable
    {
        [SerializeField] private List<SkillActionSlot> slotList = new();


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(false, slotList);

            var cb                = GetComponentInParent<CharacterBehaviour>();
            var defaultBindingKey = new List<BindingCode>
            {
                BindingCode.Q, 
                BindingCode.W, 
                BindingCode.E, 
                BindingCode.R
            };

            var skillListIndex = new List<DataIndex>
            {
                cb.SkillBehaviour.SkillList[0].DataIndex,
                cb.SkillBehaviour.SkillList[1].DataIndex,
                cb.SkillBehaviour.SkillList[2].DataIndex,
                cb.SkillBehaviour.SkillList[3].DataIndex,
            };

            slotList.ForEach((slot, index) => 
                slot.EditorPersonalSetUp(defaultBindingKey[index], skillListIndex[index]));
        }
#endif
    }
}
