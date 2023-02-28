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
        

        public void Initialize(IEnumerable<AdventurerBehaviour> adventurerList)
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
