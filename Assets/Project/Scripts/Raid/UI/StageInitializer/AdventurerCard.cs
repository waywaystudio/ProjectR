using System.Collections.Generic;
using Adventurers;
using Character.Adventurers;
using Common;
using Raid.UI.StageSetter;
using Spine.Unity;
using TMPro;
using UnityEngine;

namespace Raid.UI.StageInitializer
{
    public class AdventurerCard : MonoBehaviour, IEditable
    {
        [SerializeField] private DataIndex initialAdventurerCode;
        [SerializeField] private TextMeshProUGUI headerTextUI;
        [SerializeField] private TextMeshProUGUI nameTextUI;
        [SerializeField] private SkeletonGraphic portrait;
        [SerializeField] private List<PreviewActionSlot> previewActionSlotList;

        private Adventurer currentAdventurer;

        public DataIndex TargetAdventurerIndex => currentAdventurer.ActionCode; 


        public void SetCardUI(DataIndex adventurerCode)
        {
            if (!MainAdventurer.TryGetAdventurer(adventurerCode, out var abAdventurerObject)) return;
            if (!abAdventurerObject.TryGetComponent(out currentAdventurer)) return;

            headerTextUI.text          = currentAdventurer.Role.ToString();
            nameTextUI.text            = currentAdventurer.Name;
            // portrait.skeletonDataAsset = currentAdventurer.Animating.DataAsset;
            
            previewActionSlotList.ForEach((actionSlot, index) =>
            {
                actionSlot.Initialize(currentAdventurer.CharacterAction.SkillList[index]);
            });
        }

        public void GetNextAdventurer()
        {
            if (currentAdventurer.IsNullOrEmpty()) return;

            var nextAdventurer      = MainAdventurer.GetNext(currentAdventurer.gameObject);
            var nextAdventurerIndex = nextAdventurer.GetComponent<IDataIndexer>().ActionCode;

            SetCardUI(nextAdventurerIndex);
        }
        
        public void GetPrevAdventurer()
        {
            if (currentAdventurer.IsNullOrEmpty()) return;

            var prevAdventurer      = MainAdventurer.GetPrev(currentAdventurer.gameObject);
            var prevAdventurerIndex = prevAdventurer.GetComponent<IDataIndexer>().ActionCode;

            SetCardUI(prevAdventurerIndex);
        }


        private void Awake()
        {
            SetCardUI(initialAdventurerCode);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            headerTextUI = transform.Find("Header").Find("Text").GetComponent<TextMeshProUGUI>();
            nameTextUI   = transform.Find("Name").GetComponent<TextMeshProUGUI>();
            portrait     = GetComponentInChildren<SkeletonGraphic>();

            GetComponentsInChildren(previewActionSlotList);
        }
#endif
    }
}
