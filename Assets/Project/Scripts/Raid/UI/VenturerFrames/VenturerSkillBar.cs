using System.Collections.Generic;
using Character.Venturers;
using UnityEngine;

namespace Raid.UI.VenturerFrames
{
    public class VenturerSkillBar : MonoBehaviour, IEditable
    {
        [SerializeField] private List<VenturerSkillSlot> slotList;

        private static VenturerBehaviour FocusVenturer => RaidDirector.FocusVenturer;


        public void Initialize()
        {
            RaidDirector.VenturerList.ForEach(venturer =>
            {
                if (venturer.IsNullOrEmpty()) return;
                
                venturer.SkillTable.OnSkillSetChanged.Add("UpdateSkillActionBar", UpdateSlotList);
            });
            
            slotList.ForEach(slot => slot.Initialize());
        }

        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            UpdateSlotList();
        }


        private void UpdateSlotList()
        {
            var skills = FocusVenturer.SkillTable.SkillIndexList;
            
            slotList.ForEach((slot, index) =>
            {
                if (skills.Count <= index) return;
                
                slot.UpdateSlot(skills[index]);
            });
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(false, slotList);

            slotList.ForEach(slot => slot.EditorSetUp());
        }
#endif
    }
}
