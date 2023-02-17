using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Raid.UI.ActionBars.CharacterSkills
{
    public class CharacterSkillBar : MonoBehaviour
    {
        [SerializeField] private List<CharacterSkillActionSlot> slotList = new();
        
        public List<CharacterSkillActionSlot> SlotList => slotList;


        public void Initialize(AdventurerBehaviour ab)
        {
            GetComponentsInChildren(true, slotList);
            
            SlotList.ForEach(slot => slot.Initialize(ab));
        }


        public void SetUp()
        {
            GetComponentsInChildren(true, slotList);
        }
    }
}
