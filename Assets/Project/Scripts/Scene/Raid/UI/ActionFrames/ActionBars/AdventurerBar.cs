using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Raid.UI.ActionFrames.ActionBars
{
    using AdventurerBars;
    
    public class AdventurerBar : MonoBehaviour
    {
        [SerializeField] private List<AdventurerActionSlot> slotList = new();
        

        public void Initialize()
        {
            GetComponentsInChildren(true, slotList);
            
            RaidDirector.AdventurerList.ForEach((adventurer, index) =>
            {
                slotList[index].Initialize(adventurer);
                slotList[index].gameObject.SetActive(true);
            });
        }
    }
}
