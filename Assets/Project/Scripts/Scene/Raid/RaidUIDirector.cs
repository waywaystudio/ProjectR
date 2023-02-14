using System;
using System.Collections.Generic;
using Character;
using Core;
using UnityEngine;

namespace Raid
{
    using UI;
    using UI.MonsterFrames;
    using UI.Nameplates;
    
    public class RaidUIDirector : MonoBehaviour
    {
        [SerializeField] private List<PartyUnitFrame> partyFrameList;
        [SerializeField] private List<BossFrame> bossFrameList;
        // [SerializeField] private List<Nameplate> nameplateList;
        [SerializeField] private List<SkillSlotFrame> skillSlotFrameList;

        private AdventurerBehaviour focusAdventurer;

        public List<PartyUnitFrame> PartyFrameList => partyFrameList;
        public List<BossFrame> BossFrameList => bossFrameList;
        // public List<Nameplate> NameplateList => nameplateList;
        public List<SkillSlotFrame> SkillSlotFrameList => skillSlotFrameList;

        public AdventurerBehaviour FocusAdventurer
        {
            get => focusAdventurer;
            set
            {
                if (value == focusAdventurer) return;
                
                focusAdventurer = value;
                skillSlotFrameList.ForEach(x => x.Unregister());
                skillSlotFrameList.ForEach(x => x.Register(focusAdventurer));
            }
        }

        public void Initialize(List<AdventurerBehaviour> adventurerList, List<MonsterBehaviour> monsterList)
        {
            Initialize(adventurerList);
            Initialize(monsterList);
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
        
        private void Initialize(List<MonsterBehaviour> monsterList)
        {
            if (BossFrameList.IsNullOrEmpty() || monsterList.IsNullOrEmpty()) return;
            
            monsterList.ForEach((x, i) =>
            {
                if (BossFrameList.Count > i) BossFrameList[i].Initialize(x);
            });
        }

        private void Awake()
        {
            SetUp();
        }

        private void Update()
        {
            // nameplateList.ForEach(x => x.UpdatePlate());
        }

#if UNITY_EDITOR
        public void SetUp()
        {
            GetComponentsInChildren(true, PartyFrameList);
            GetComponentsInChildren(true, BossFrameList);
            // GetComponentsInChildren(NameplateList);
            GetComponentsInChildren(true, SkillSlotFrameList);
        }
#endif
    }
}
