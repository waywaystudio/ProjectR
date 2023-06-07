using System.Collections.Generic;
using Common;
using Common.Camps;
using DG.Tweening;
using UnityEngine;

namespace Lobby.UI.Forge
{
    public class OldAdventurerFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private List<OldAdventurerStatusUI> adventurerStatusUiList;
        [SerializeField] private RectTransform contentsRect;
        
        private bool OnMoving { get; set; }


        public void GetPreviousAdventurerInfo()
        {
            var focusAdventurer = LobbyDirector.UI.Forge.FocusVenturer;
            if (OnMoving || focusAdventurer == VenturerType.Knight) return;
            
            OnMoving = true;
            
            var targetValue = Mathf.Max(0f, contentsRect.localPosition.y - 550f);
            
            contentsRect.DOLocalMoveY(targetValue, 0.2f).OnComplete(() =>
            {
                OnMoving                               = false;
                LobbyDirector.UI.Forge.FocusVenturer = GetPrevAdventurer(focusAdventurer);
            });
        }
        
        public void GetNextAdventurerInfo()
        {
            // var focusAdventurer = LobbyDirector.UI.Forge.FocusVenturer;
            // if (OnMoving || focusAdventurer == VenturerType.Ranger) return;
            //
            // OnMoving = true;
            //
            // var targetValue = Mathf.Min((Camp.Characters.GetAllData().Count - 1) * 550f,
            //                             contentsRect.localPosition.y + 550f);
            //
            // contentsRect.DOLocalMoveY(targetValue, 0.2f).OnComplete(() =>
            // {
            //     OnMoving                               = false;
            //     LobbyDirector.UI.Forge.FocusVenturer = GetNextAdventurer(focusAdventurer);
            // });
        }
        
        
        // TODO. temporary func.
        // 현재 AdventurerUI가 3개 뿐이며, 스크롤이동방식 때문에 .Next() 함수를 사용할 수 없음.
        private VenturerType GetNextAdventurer(VenturerType currentClass) => currentClass switch
        {
            VenturerType.Knight => VenturerType.Rogue,
            VenturerType.Rogue  => VenturerType.Ranger,
            _                   => VenturerType.None,
        };
        private VenturerType GetPrevAdventurer(VenturerType currentClass) => currentClass switch
        {
            VenturerType.Ranger => VenturerType.Rogue,
            VenturerType.Rogue  => VenturerType.Knight,
            _                   => VenturerType.None,
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
