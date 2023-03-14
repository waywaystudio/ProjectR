using System.Collections.Generic;
using Character.Adventurers;
using UnityEngine;

namespace Raid.UI.ActionFrames
{
    public class SkillActionBar : MonoBehaviour
    {
        [SerializeField] protected string barName;
        [SerializeField] protected int slotCount;
        [SerializeField] private List<SkillActionSlot> slotList = new();
        
        public List<SkillActionSlot> SlotList => slotList;


        public void Initialize(Adventurer ab)
        {
            GetComponentsInChildren(true, slotList);
            
            SlotList.ForEach(slot => slot.Initialize(ab));
            SlotList.ForEach((slot, index) =>
            {
                if (ab.CharacterAction.SkillAction.SkillList.ElementCount() <= index) return;
                
                var skill = ab.CharacterAction.SkillAction.SkillList[index];
                slot.Initialize(ab.CharacterAction, skill);
            });
        }
    }
}
