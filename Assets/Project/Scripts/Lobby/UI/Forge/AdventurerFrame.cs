using System.Collections.Generic;
using Common;
using Common.PartyCamps;
using DG.Tweening;
using UnityEngine;

namespace Lobby.UI.Forge
{
    public class AdventurerFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private List<AdventurerStatusUI> adventurerStatusUiList;
        [SerializeField] private RectTransform contentsRect;
        
        private bool OnMoving { get; set; }


        public void GetPreviousAdventurerInfo()
        {
            var focusAdventurer = LobbyDirector.UI.Forge.FocusAdventurer;
            if (OnMoving || focusAdventurer == CombatClassType.Knight) return;
            
            OnMoving = true;
            
            var targetValue = Mathf.Max(0f, contentsRect.localPosition.y - 550f);
            
            contentsRect.DOLocalMoveY(targetValue, 0.2f).OnComplete(() =>
            {
                OnMoving                               = false;
                LobbyDirector.UI.Forge.FocusAdventurer = GetPrevAdventurer(focusAdventurer);
            });
        }
        
        public void GetNextAdventurerInfo()
        {
            var focusAdventurer = LobbyDirector.UI.Forge.FocusAdventurer;
            if (OnMoving || focusAdventurer == CombatClassType.Ranger) return;
            
            OnMoving = true;
            
            var targetValue = Mathf.Min((PartyCamp.Characters.GetAllData().Count - 1) * 550f,
                                        contentsRect.localPosition.y + 550f);
            
            contentsRect.DOLocalMoveY(targetValue, 0.2f).OnComplete(() =>
            {
                OnMoving                               = false;
                LobbyDirector.UI.Forge.FocusAdventurer = GetNextAdventurer(focusAdventurer);
            });
        }
        
        
        // TODO. temporary func.
        // 현재 AdventurerUI가 3개 뿐이며, 스크롤이동방식 때문에 .Next() 함수를 사용할 수 없음.
        private CombatClassType GetNextAdventurer(CombatClassType currentClass) => currentClass switch
        {
            CombatClassType.Knight => CombatClassType.Rogue,
            CombatClassType.Rogue  => CombatClassType.Ranger,
            _ => CombatClassType.None,
        };
        private CombatClassType GetPrevAdventurer(CombatClassType currentClass) => currentClass switch
        {
            CombatClassType.Ranger => CombatClassType.Rogue,
            CombatClassType.Rogue  => CombatClassType.Knight,
            _                      => CombatClassType.None,
        };
        //


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(adventurerStatusUiList);
        }
#endif
    }
}
