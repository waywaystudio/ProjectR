using Common.Equipments;

namespace Common.UI.Tooltips
{
    public class EquipmentTooltipPool : Pool<EquipmentTooltip>
    {
        public EquipmentTooltip ShowToolTip(UnityEngine.Vector3 position, Equipment equipment)
        {
            var tooltipObject = Get();

            tooltipObject.transform.position = position;
            tooltipObject.Show(equipment);

            return tooltipObject;
        }
        
        protected override void OnGetPool(EquipmentTooltip element)
        {
            element.gameObject.SetActive(true);
            element.transform.SetParent(Origin);
        }

        protected override void OnReleasePool(EquipmentTooltip element)
        {
            element.Hide();
            element.gameObject.SetActive(false);
            element.transform.SetParent(Origin);
        }

        protected override void OnDestroyPool(EquipmentTooltip element)
        {
            Destroy(element.gameObject);
        }
    }
}
