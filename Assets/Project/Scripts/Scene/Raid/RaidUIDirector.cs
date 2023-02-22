using System.Collections.Generic;
using Character;
using Core;
using Raid.UI.ActionFrames;
using Raid.UI.ActionFrames.ActionBars.CharacterSkillBars;
using UnityEngine;

namespace Raid
{
    using UI;
    using UI.BossFrames;
    using UI.ActionFrames.ActionBars;
    using UI.ActionFrames.ActionBars.AdventurerBars;
    using UI.ActionFrames.StatusEffectIconBars;

    public class RaidUIDirector : MonoBehaviour
    {
        // TODO.TEMP
        [SerializeField] private AdventurerBehaviour player;
        //
        
        // Boss Frames
        [SerializeField] private BossHpProcess bossHpProcess;
        
        // Action Frames
        [SerializeField] private CastingProgress castingProgress;
        [SerializeField] private StatusEffectBar statusEffectBar;
        [SerializeField] private DynamicValueProcess dynamicValueProcess;
        [SerializeField] private CharacterDashActionSlot dashAction;
        [SerializeField] private AdventurerBar adventurerBar;
        [SerializeField] private ActionBar actionBar;
        
        // Pool
        [SerializeField] private DamageTextPool damageTextPool;
        
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
            statusEffectBar.Initialize(player);
            dynamicValueProcess.Initialize(player);
            actionBar.Initialize(player);
            dashAction.Initialize(player);
        }

        public void Initialize(List<AdventurerBehaviour> adventurerList, List<MonsterBehaviour> monsterList)
        {
            Initialize(player);
            Initialize(adventurerList);
            
            bossHpProcess.Initialize(monsterList[0]);
            adventurerBar.Initialize(adventurerList);
            damageTextPool.Initialize(adventurerList);
        }
        

        private void Initialize(List<AdventurerBehaviour> adventurerList)
        {
            if (PartyFrameList.IsNullOrEmpty() ||
                adventurerList.IsNullOrEmpty()) return;
            
            adventurerList.ForEach((x, i) =>
            {
                if (PartyFrameList.Count > i) PartyFrameList[i].Initialize(x);
            });
        }

        private void Awake()
        {
            SetUp();
        }


#if UNITY_EDITOR
        public void SetUp()
        {
            statusEffectBar ??= GetComponentInChildren<StatusEffectBar>();
            dynamicValueProcess ??= GetComponentInChildren<DynamicValueProcess>();
            actionBar      ??= GetComponentInChildren<ActionBar>();

            GetComponentsInChildren(true, PartyFrameList);
        }
#endif
    }
}
