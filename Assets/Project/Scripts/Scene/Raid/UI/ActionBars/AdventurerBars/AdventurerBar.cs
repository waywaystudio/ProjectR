using System.Collections.Generic;
using Character;
using Core;
using UnityEngine;

namespace Raid.UI.ActionBars.AdventurerBars
{
    public class AdventurerBar : MonoBehaviour
    {
        [SerializeField] private List<AdventurerActionSlot> slotList = new();
        
        public List<AdventurerActionSlot> SlotList => slotList;

        public void Initialize(List<AdventurerBehaviour> adventurerList)
        {
            GetComponentsInChildren(true, slotList);
            
            adventurerList.ForEach((adventurer, index) =>
            {
                slotList[index].Initialize(adventurer);
                slotList[index].gameObject.SetActive(true);
            });
        }


        public void SetUp()
        {
            GetComponentsInChildren(true, slotList);
        }
    }
}
