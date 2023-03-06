using System.Collections.Generic;
using Character;
using Core;
using Raid.UI.ActionFrames.ActionBars.AdventurerBars;
using UnityEngine;

namespace Raid.UI.ActionFrames.ActionBars
{
    public class AdventurerBar : MonoBehaviour
    {
        [SerializeField] private List<AdventurerActionSlot> slotList = new();
        

        public void Initialize(IEnumerable<Adventurer> adventurerList)
        {
            GetComponentsInChildren(true, slotList);
            
            adventurerList.ForEach((adventurer, index) =>
            {
                if (index >= slotList.Count) return;
                
                slotList[index].Initialize(adventurer);
                slotList[index].gameObject.SetActive(true);
            });
        }


        public void EditorSetUp()
        {
            GetComponentsInChildren(true, slotList);
        }
    }
}
