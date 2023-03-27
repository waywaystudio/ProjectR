using Common.UI.Tooltips;
using Singleton;
using UI;
using UnityEngine;

public class MainUI : MonoSingleton<MainUI>
{
    [SerializeField] private FadePanel fadePanel;
    [SerializeField] private TooltipPool tooltipPool;

    public static FadePanel FadePanel => Instance.fadePanel;
    public static TooltipPool TooltipPool => Instance.tooltipPool;
}