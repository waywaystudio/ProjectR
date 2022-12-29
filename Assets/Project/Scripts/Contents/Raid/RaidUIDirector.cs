using System.Collections.Generic;
using Common.Character;
using Core;
using Core.GameEvents;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Raid
{
    using UI;
    
    public class RaidUIDirector : MonoBehaviour
    {
        [SerializeField] private RaidDirector raidDirector;
        [SerializeField] private GameEventTransform cameraFocusChanging;

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
                cameraFocusChanging.Invoke(focusAdventurer.transform);
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
        [Button]
        private void SetUp()
        {
            raidDirector = GetComponentInParent<RaidDirector>();
        }
#endif
    }
}
