using Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class RelicSelectUI : MonoBehaviour, IPointerClickHandler, IEditable
    {
        [SerializeField] private RelicType relicType;
        [SerializeField] private Image iconImage;
        [SerializeField] private Image boarderImage;
        [SerializeField] private Color selectedImageColor = Color.white;
        [SerializeField] private Color unselectedImageColor = new (1, 1, 1, 0.3f);
        
        private bool IsFocused { get; set; }

        public void OnPointerClick(PointerEventData eventData)
        {
            LobbyDirector.UI.Forge.FocusRelic = relicType;
        }

        public void OnSelected()
        {
            if (relicType == LobbyDirector.UI.Forge.FocusRelic)
            {
                if (IsFocused) return;

                IsFocused = true;
                Select();
            }
            else
            {
                if (!IsFocused) return;
                
                IsFocused = false;
                Unselect();
            }
        }
        

        private void Select()
        {
            boarderImage.enabled = true;
            iconImage.color      = selectedImageColor;
        }

        private void Unselect()
        {
            boarderImage.enabled = false;
            iconImage.color      = unselectedImageColor;
        }

        private void Awake()
        {
            Unselect();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            iconImage    = transform.Find("Icon").Find("Contents").GetComponent<Image>();
            boarderImage = transform.Find("Icon").Find("Boarder").GetComponent<Image>();
        }
#endif
        
    }
}
