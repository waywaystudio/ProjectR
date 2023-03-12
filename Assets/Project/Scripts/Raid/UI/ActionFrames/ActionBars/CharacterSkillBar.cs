using System.Collections.Generic;
using Character;
using Character.Adventurers;
using Raid.UI.ActionFrames.ActionBars.CharacterSkillBars;
using UnityEngine;

namespace Raid.UI.ActionFrames.ActionBars
{
    public class CharacterSkillBar : MonoBehaviour
    {
        [SerializeField] private List<CharacterSkillActionSlot> slotList = new();
        
        public List<CharacterSkillActionSlot> SlotList => slotList;


        public void Initialize(Adventurer ab)
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
