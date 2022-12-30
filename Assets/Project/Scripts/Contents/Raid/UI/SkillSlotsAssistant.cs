using System.Collections.Generic;
using UnityEngine;

namespace Raid.UI
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
