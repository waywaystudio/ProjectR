using System.Collections.Generic;
using Character;
using Core;
using Raid.UI.ActionBars.AdventurerBars;
using Raid.UI.BossFrames;
using UnityEngine;

namespace Raid
{
    using UI;
    using UI.ActionBars;
    using UI.DynamicValueProcesses;
    using UI.StatusEffectIconBars;
    using UI.CastingProgress;
    using UI.ActionBars.CharacterSkills;
    
    public class RaidUIDirector : MonoBehaviour
    {
        // TODO.TEMP
        [SerializeField] private AdventurerBehaviour player;
        //
        
        // Boss Frames
        [SerializeField] private BossHpProcess bossHpProcess;
        
        // Player Frames
        [SerializeField] private CastingProgress castingProgress;
        [SerializeField] private StatusEffectIconBar statusEffectIconBar;
        [SerializeField] private DynamicValueProcess dynamicValueProcess;
        [SerializeField] private CharacterDashActionSlot dashAction;
        [SerializeField] private AdventurerBar adventurerBar;
        [SerializeField] private ActionBarFrame actionBarFrame;
        
        [SerializeField] private List<PartyUnitFrame> partyFrameList;

        private AdventurerBehaviour focusAdventurer;

        public List<PartyUnitFrame> PartyFrameList => partyFrameList;

        public AdventurerBehaviour FocusAdventurer
        {
            get => focusAdventurer;
            set
            {
                if (focusAdventurer != null && value == focusAdventurer) return;
                
                focusAdventurer = value;
            }
        }


        public void Initialize(AdventurerBehaviour player)
        {
            castingProgress.Initialize(player);
            statusEffectIconBar.Initialize(player);
            dynamicValueProcess.Initialize(player);
            actionBarFrame.Initialize(player);
            dashAction.Initialize(player);
        }

        public void Initialize(List<AdventurerBehaviour> adventurerList, List<MonsterBehaviour> monsterList)
        {
            Initialize(player);
            Initialize(adventurerList);
            
            bossHpProcess.Initialize(monsterList[0]);
            adventurerBar.Initialize(adventurerList);
        }
        

        private void Initialize(List<AdventurerBehaviour> adventurerList)
        {
            if (PartyFrameList.IsNullOrEmpty() ||
                // NameplateList.IsNullOrEmpty() ||
                adventurerList.IsNullOrEmpty()) return;
            
            adventurerList.ForEach((x, i) =>
            {
                if (PartyFrameList.Count > i) PartyFrameList[i].Initialize(x);
                // if (NameplateList.Count > i) NameplateList[i].Initialize(x);
            });
        }

        private void Awake()
        {
            SetUp();
        }


#if UNITY_EDITOR
        public void SetUp()
        {
            statusEffectIconBar ??= GetComponentInChildren<StatusEffectIconBar>();
            dynamicValueProcess ??= GetComponentInChildren<DynamicValueProcess>();
            actionBarFrame      ??= GetComponentInChildren<ActionBarFrame>();

            GetComponentsInChildren(true, PartyFrameList);
            // GetComponentsInChildren(NameplateList);
        }
#endif
    }
}
