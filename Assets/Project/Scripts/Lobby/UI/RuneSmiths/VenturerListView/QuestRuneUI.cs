using Common.UI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lobby.UI.RuneSmiths.VenturerListView
{
    public class QuestRuneUI : MonoBehaviour, IPointerClickHandler, IEditable
    {
        [SerializeField] private ImageFiller progressBar;
        [SerializeField] private Image statusSymbol;
        [SerializeField] private TextMeshProUGUI questTitle;
        [SerializeField] private TextMeshProUGUI achieve;
        

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"{questTitle}.Clicked");
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            progressBar  = GetComponentInChildren<ImageFiller>();
            statusSymbol = transform.Find("StatusSymbol").Find("Symbol").GetComponent<Image>();
            questTitle   = transform.Find("QuestRuneTitle").Find("Title").GetComponent<TextMeshProUGUI>();
            achieve   = transform.Find("Achieve").GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
