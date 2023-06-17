using Common.Equipments;
using UnityEngine;

namespace Common.UI.Tooltips
{
    public class EquipmentTooltipPool : MonoBehaviour 
    {
        [SerializeField] private Pool<EquipmentTooltip> pool;
        
        public EquipmentTooltip ShowToolTip(Vector3 position, EquipmentEntity equipment)
        {
            var tooltipObject = pool.Get();
        
            tooltipObject.transform.position = position;
            tooltipObject.Show(equipment);
        
            return tooltipObject;
        }


        private void ToolTipInitialize(EquipmentTooltip tooltip)
        {
            tooltip.OnEnded.Add("ReleasePool", () => pool.Release(tooltip));
        }

        private void Awake()
        {
            pool.Initialize(ToolTipInitialize, transform);
        }
    }
}
