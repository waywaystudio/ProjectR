using System.Collections.Generic;
using Common;
using Common.Characters;
using Common.PartyCamps;
using DG.Tweening;
using UnityEngine;

namespace Lobby.UI
{
    public class AdventurerFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private List<AdventurerStatusUI> adventurerStatusUiList;
        [SerializeField] private RectTransform contentsRect;

        public CharacterData CurrentAdventurerData { get; private set; }
        
        private bool OnMoving { get; set; }


        public void ReloadAdventurer(DataIndex type)
        {
            foreach (var adventurerStatusUi in adventurerStatusUiList)
            {
                if (adventurerStatusUi.DataIndex != type) continue;
                
                adventurerStatusUi.Reload();
                break;
            }
        }

        public void GetPreviousAdventurerInfo()
        {
            if (OnMoving || CurrentAdventurerData.ClassType == CombatClassType.Knight) return;
            
            OnMoving = true;
            
            var targetValue = Mathf.Max(0f, contentsRect.localPosition.y - 550f);
            
            contentsRect.DOLocalMoveY(targetValue, 0.2f).OnComplete(() => OnMoving = false);
            CurrentAdventurerData = PartyCamp.Characters.GetPreviousData(CurrentAdventurerData);
        }
        
        public void GetNextAdventurerInfo()
        {
            if (OnMoving || CurrentAdventurerData.ClassType == CombatClassType.Ranger) return;
            
            OnMoving = true;
            
            var targetValue = Mathf.Min((PartyCamp.Characters.GetAllData().Count - 1) * 550f,
                                        contentsRect.localPosition.y + 550f);
            
            contentsRect.DOLocalMoveY(targetValue, 0.2f).OnComplete(() => OnMoving = false);
            CurrentAdventurerData = PartyCamp.Characters.GetNextData(CurrentAdventurerData);
        }


        private void Awake()
        {
            CurrentAdventurerData = PartyCamp.Characters.GetData(DataIndex.Knight);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(adventurerStatusUiList);
        }
#endif
    }
}
