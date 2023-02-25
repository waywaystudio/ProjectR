using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Raid.UI.ActionFrames.ActionBars
{
    using CharacterSkillBars;
    
    public class CharacterSkillBar : MonoBehaviour, IEditable
    {
        [SerializeField] private List<CharacterSkillActionSlot> slotList = new();
        
        public List<CharacterSkillActionSlot> SlotList => slotList;


        public void Initialize()
        {
            SlotList.ForEach(slot => slot.Initialize());
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(true, slotList);
        }
#endif
    }
}
