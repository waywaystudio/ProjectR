using System.Collections.Generic;
using Character.Venturers;
using Common.Characters.Behaviours;
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
                
                venturer.SkillTable.OnSkillTableChanged.Add("RegisterSlotList", RegisterSlotList);
            });
            
            slotList.ForEach(slot => slot.Initialize());
        }

        public void OnFocusVenturerChanged(VenturerBehaviour vb)
        {
            if (vb == null) return;

            RegisterSlotList(vb.SkillTable);
        }

        public void OnCommandModeEnter()
        {
            SetActiveSkillSlots(false);
            slotList.ForEach(slot => slot.RemoveInput());
            // 새로운 스킬바가 있을 예정
        }
        
        public void OnCommandModeExit()
        {
            SetActiveSkillSlots(true);
            slotList.ForEach(slot => slot.AddInput());
            // 새로운 스킬바가 있을 예정
        }


        private void RegisterSlotList(SkillTable table)
        {
            // UI 변경이 필요 없으면,
            if (FocusVenturer != table.Cb) return;
            
            var skills = table.SkillIndexList;
            
            slotList.ForEach((slot, index) =>
            {
                if (skills.Count <= index) return;
                
                slot.UpdateSkill(skills[index]);
            });
        }

        private void SetActiveSkillSlots(bool activity)
        {
            slotList.ForEach(slot =>
            {
                if (slot.isActiveAndEnabled == activity) return;

                slot.gameObject.SetActive(activity);
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
