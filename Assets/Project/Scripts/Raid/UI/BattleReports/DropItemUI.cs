using Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.BattleReports
{
    public class DropItemUI : MonoBehaviour, IEditable
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI title;


        public void SetItem(IReward item)
        {
            // frame = SpriteManager.GetFrame(item.Grade);
            icon.sprite = item.Icon;
            title.text  = item.Title;
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            icon  = transform.Find("Icon").GetComponent<Image>();
            title = transform.Find("Title").GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
