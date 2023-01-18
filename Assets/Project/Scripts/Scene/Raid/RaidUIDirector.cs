using System.Collections.Generic;
using Character;
using Core;
using Scene.Raid.UI;
using UnityEngine;

namespace Scene.Raid
{
    public class RaidUIDirector : MonoBehaviour
    {
        [SerializeField] private RaidDirector raidDirector;
        [SerializeField] private List<PartyUnitFrame> partyFrameList;
        [SerializeField] private List<SkillSlotFrame> skillSlotFrameList;

        private AdventurerBehaviour focusAdventurer;

        public List<PartyUnitFrame> PartyFrameList => partyFrameList;
        public List<SkillSlotFrame> SkillSlotFrameList => skillSlotFrameList;

        public AdventurerBehaviour FocusAdventurer
        {
            get => focusAdventurer;
            set
            {
                if (value == focusAdventurer) return;
                
                focusAdventurer = value;
                raidDirector.OnFocusCharacterChanged.Invoke(value.transform);
                SkillSlotFrameList.ForEach(x => x.Unregister());
                SkillSlotFrameList.ForEach(x => x.Register(focusAdventurer));
            }
        }

        public void Initialize(List<AdventurerBehaviour> adventurerList)
        {
            if (PartyFrameList.IsNullOrEmpty() || adventurerList.IsNullOrEmpty()) return;
            
            adventurerList.ForEach((x, i) => PartyFrameList[i].Initialize(x));
        }


#if UNITY_EDITOR
        public void SetUp()
        {
            GetComponentsInChildren(partyFrameList);
            GetComponentsInChildren(skillSlotFrameList);
        }
#endif
    }
}
