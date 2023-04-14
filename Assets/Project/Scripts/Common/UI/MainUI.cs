using Singleton;
using UnityEngine;

namespace Common.UI
{
    using Tooltips;
    
    public class MainUI : MonoSingleton<MainUI>
    {
        [SerializeField] private FadePanel fadePanel;
        [SerializeField] private TooltipPool tooltipPool;
        [SerializeField] private EquipmentTooltipPool equipmentTooltipPool;

        public static FadePanel FadePanel => Instance.fadePanel;
        public static TooltipPool TooltipPool => Instance.tooltipPool;
        public static EquipmentTooltipPool EquipmentTooltipPool => Instance.equipmentTooltipPool;
    }
}