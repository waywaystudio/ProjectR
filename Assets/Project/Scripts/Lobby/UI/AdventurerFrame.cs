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
        public CharacterBehaviour CurrentCharacter { get; private set; }


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
            if (CurrentCharacter.CombatClass == CombatClassType.Knight) return;
            if (OnMoving) return;
            
            OnMoving = true;
            
            var targetValue = Mathf.Max(0f, contentsRect.localPosition.y - 550f);

            contentsRect.DOLocalMoveY(targetValue, 0.2f).OnComplete(() => OnMoving = false);;
            CurrentCharacter = PlayerCamp.Characters.GetPreviousCharacter(CurrentCharacter.CombatClass);
        }
        
        public void GetNextAdventurerInfo()
        {
            if (CurrentCharacter.CombatClass == CombatClassType.Hunter) return;
            if (OnMoving) return;

            OnMoving = true;
            
            var targetValue = Mathf.Min((PlayerCamp.Characters.GetAllCharacters().Count - 1) * 550f,
                contentsRect.localPosition.y + 550f);

            contentsRect.DOLocalMoveY(targetValue, 0.2f).OnComplete(() => OnMoving = false);
            CurrentCharacter = PlayerCamp.Characters.GetNextCharacter(CurrentCharacter.CombatClass);
        }


        private void Awake()
        {
            CurrentCharacter = PlayerCamp.Characters.Get(CombatClassType.Knight);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(adventurerStatusUiList);
        }
#endif
    }
}
