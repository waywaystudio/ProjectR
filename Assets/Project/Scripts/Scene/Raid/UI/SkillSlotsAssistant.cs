using System.Collections.Generic;
using Raid;
using Raid.UI;
using UnityEngine;

namespace Scene.Raid.UI
{
    public class SkillSlotsAssistant : MonoBehaviour
    {
        [SerializeField] private RaidUIDirector uiDirector;
        [SerializeField] private List<SkillSlotFrame> slotFrameList;

        public void Initialize()
        {
            
        }
        

        private void SetUp()
        {
            uiDirector ??= GetComponentInParent<RaidUIDirector>();
        }
    }
}
