using Core.Singleton;
using UI;
using UI.Tooltips;
using UnityEngine;

public class MainUI : MonoSingleton<MainUI>
{
    [SerializeField] private FadePanel fadePanel;
    [SerializeField] private TooltipPool tooltipPool;

    public static FadePanel FadePanel => Instance.fadePanel;
    public static TooltipPool TooltipPool => Instance.tooltipPool;
}