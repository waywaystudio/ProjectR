using System.Collections.Generic;
using Common;
using Common.Characters;
using Common.PlayerCamps;
using DG.Tweening;
using UnityEngine;

namespace Lobby.UI
{
    public class AdventurerFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private List<AdventurerStatusUI> adventurerStatusUiList;
        [SerializeField] private RectTransform contentsRect;

        private bool OnMoving { get; set; }
        
        [Sirenix.OdinInspector.ShowInInspector]
        public CharacterData CurrentAdventurerData { get; private set; }


        public void ReloadAdventurer(CombatClassType type)
        {
            foreach (var adventurerStatusUi in adventurerStatusUiList)
            {
                if (adventurerStatusUi.ClassType != type) continue;
                
                adventurerStatusUi.Reload();
                break;
            }
        }

        public void GetPreviousAdventurerInfo()
        {
            if (CurrentAdventurerData.ClassType == CombatClassType.Knight) return;
            if (OnMoving) return;
            
            OnMoving = true;
            
            var targetValue = Mathf.Max(0f, contentsRect.localPosition.y - 550f);

            contentsRect.DOLocalMoveY(targetValue, 0.2f).OnComplete(() => OnMoving = false);;
            CurrentAdventurerData = PlayerCamp.Characters.GetPreviousData(CurrentAdventurerData);
        }
        
        public void GetNextAdventurerInfo()
        {
            if (CurrentAdventurerData.ClassType == CombatClassType.Ranger) return;
            if (OnMoving) return;

            OnMoving = true;
            
            var targetValue = Mathf.Min((PlayerCamp.Characters.GetAllData().Count - 1) * 550f,
                contentsRect.localPosition.y + 550f);

            contentsRect.DOLocalMoveY(targetValue, 0.2f).OnComplete(() => OnMoving = false);
            CurrentAdventurerData = PlayerCamp.Characters.GetNextData(CurrentAdventurerData);
        }


        private void Awake()
        {
            CurrentAdventurerData = PlayerCamp.Characters.GetData(CombatClassType.Knight);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(adventurerStatusUiList);
        }
#endif
    }
}
