using Core.Singleton;
using UnityEngine;

namespace MainGame
{
    using UI;
    using UI.Tooltips;
    
    public class MainUI : MonoSingleton<MainUI>
    {
        [SerializeField] private FadePanel fadePanel;
        [SerializeField] private TooltipPool tooltipPool;

        public static FadePanel FadePanel => Instance.fadePanel;
        public static TooltipPool TooltipPool => Instance.tooltipPool;
    }
}
