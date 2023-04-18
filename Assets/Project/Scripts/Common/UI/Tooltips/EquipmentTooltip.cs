using System.Collections.Generic;
using Common.Equipments;
using TMPro;
using UnityEngine;

namespace Common.UI.Tooltips
{
    public class EquipmentTooltip : MonoBehaviour, IEndSection, IEditable
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private List<StatInfoUI> statInfoList;
        [SerializeField] private TextMeshProUGUI effect;

        public ActionTable OnEnded { get; } = new();

        public void Show(Equipment equipment)
        {
            if (equipment == null) return;

            title.text = equipment.Title;
            statInfoList.ForEach(statInfoUI => statInfoUI.gameObject.SetActive(false));
            
            equipment.Spec.Iterate(stat =>
            {
                var statInfo = statInfoList.TryGetElement(info => info.StatType == stat.StatType);

                if (statInfo != null)
                {
                    statInfo.SetValue(stat);
                    statInfo.gameObject.SetActive(true);
                }
            });
        }

        public void Hide()
        {
            statInfoList.ForEach(statInfoUI => statInfoUI.gameObject.SetActive(false));
            OnEnded.Invoke();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(true, statInfoList);
            
            title  = transform.Find("Tooltip").Find("Title").GetComponent<TextMeshProUGUI>();
            effect = transform.Find("Tooltip").Find("Effect").GetComponent<TextMeshProUGUI>();
        }
#endif
        
    }
}
