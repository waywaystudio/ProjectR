using System.Collections.Generic;
using UnityEngine;

namespace Raid.UI.ActionBars.CharacterSkills
{
    public class CharacterSkillBar : MonoBehaviour
    {
        [SerializeField] private List<CharacterSkillActionSlot> slotList = new();
        
        public List<CharacterSkillActionSlot> SlotList => slotList;

        protected void Awake()
        {
            GetComponentsInChildren(true, slotList);
        }
        

        public void SetUp()
        {
            GetComponentsInChildren(true, slotList);
        }
    }
}
