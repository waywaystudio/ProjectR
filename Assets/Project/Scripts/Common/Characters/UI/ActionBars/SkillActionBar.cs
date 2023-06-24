using System.Collections.Generic;
using UnityEngine;

namespace Common.Characters.UI.ActionBars
{
    public class SkillActionBar : MonoBehaviour, IEditable
    {
        [SerializeField] private List<SkillActionSlot> slotList = new();

        private CharacterBehaviour cb;
        public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();


        private void UpdateSlotList()
        {
            var skills = Cb.SkillBehaviour.SkillIndexList;
            
            slotList.ForEach((slot, index) =>
            {
                if (skills.Count <= index) return;
                
                slot.UpdateSlot(skills[index]);
            });
        }

        private void Awake()
        {
            Cb.SkillBehaviour.OnSkillChanged.Add("UpdateSkillActionBar", UpdateSlotList);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(false, slotList);

            var skills = Cb.SkillBehaviour.SkillIndexList;
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
