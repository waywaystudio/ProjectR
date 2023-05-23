using Common;
using Common.Equipments;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class DropItemUI : MonoBehaviour, IEquipmentTooltip, IEditable
    {
        [SerializeField] private Image itemImage;

        [Sirenix.OdinInspector.ShowInInspector]
        public EquipmentEntity EquipmentEntity { get; private set; }
        
        public void SetDropItemUI(EquipmentEntity equipment)
        {
            EquipmentEntity  = equipment;
            itemImage.sprite = equipment.Icon;
        }
        
        
#if UNITY_EDITOR
        public void EditorSetUp()
        {
            itemImage = transform.Find("Contents").GetComponent<Image>();
        }
#endif
    }
}
