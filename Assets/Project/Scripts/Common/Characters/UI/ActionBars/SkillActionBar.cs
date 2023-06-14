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
            var skills = cb.SkillBehaviour.SkillIndexList;
            var defaultBindingKey = new List<BindingCode>
            {
                BindingCode.Q, 
                BindingCode.W, 
                BindingCode.E, 
                BindingCode.R
            };

            slotList.ForEach((slot, index) =>
            {
                if (skills.Count <= index) return;
                
                slot.EditorPersonalSetUp(defaultBindingKey[index], skills[index]);
            });
        }
#endif
    }
}
