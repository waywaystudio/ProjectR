using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.UI.Tooltips
{
    [RequireComponent(typeof(IEquipmentTooltip))]
    public class EquipmentTooltipDrawer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Vector3 spawnOffset;
        
        private EquipmentTooltip equipmentTooltipObject;
        private IEquipmentTooltip tooltipIncludedObject;
        private IEquipmentTooltip TooltipIncludedObject => tooltipIncludedObject ??= GetComponent<IEquipmentTooltip>();
        
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Draw();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }

        public void Draw()
        {
            // Hide();
            //
            // var spawnPosition = transform.position + spawnOffset;
            //
            // if (TooltipIncludedObject.Equipment      == null 
            //     // || TooltipIncludedObject.Equipment.Info == null
            //     )
            //     return;
            //
            // equipmentTooltipObject = 
            //     MainUI.EquipmentTooltipPool.ShowToolTip(spawnPosition, TooltipIncludedObject.Equipment);
        }

        public void Hide()
        {
            // if (TooltipIncludedObject.Equipment      == null 
            //     // || TooltipIncludedObject.Equipment.Info == null 
            //     || equipmentTooltipObject == null)
            //     return;
            //
            // equipmentTooltipObject.Hide();
            // equipmentTooltipObject = null;
        }


        private void OnDisable()
        {
            if (equipmentTooltipObject.IsNullOrDestroyed() || !equipmentTooltipObject.gameObject.activeSelf) return;
            
            // equipmentTooltipObject.Hide();
        }
    }
}
