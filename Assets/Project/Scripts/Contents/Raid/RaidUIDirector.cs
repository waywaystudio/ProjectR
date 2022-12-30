using System.Collections.Generic;
using Common.Character;
using Core;
using UnityEngine;

namespace Raid
{
    using UI;
    
    public class RaidUIDirector : MonoBehaviour
    {
        [SerializeField] private RaidDirector raidDirector;

        private AdventurerBehaviour focusAdventurer;
        
        public List<PartyUnitFrame> PartyFrameList { get; } = new();
        public List<SkillSlotFrame> SkillSlotFrameList { get; } = new();

        public AdventurerBehaviour FocusAdventurer
        {
            get => focusAdventurer;
            set
            {
                if (value == focusAdventurer) return;
                
                focusAdventurer = value;
                raidDirector.OnCharacterFocusChanged.Invoke(value.transform);
                SkillSlotFrameList.ForEach(x => x.Unregister());
                SkillSlotFrameList.ForEach(x => x.Register(focusAdventurer));
            }
        }


        private void Start()
        {
            var adventurerList = raidDirector.AdventurerList;

            if (PartyFrameList.IsNullOrEmpty()) return;
            
            for (var i = 0; i < adventurerList.Count; ++i)
            {
                PartyFrameList[i].Initialize(adventurerList[i]);
            }
        }

#if UNITY_EDITOR
        private void SetUp()
        {
            raidDirector = GetComponentInParent<RaidDirector>();
        }
#endif
    }
}
