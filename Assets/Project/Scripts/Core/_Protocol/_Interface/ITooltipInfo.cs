namespace Core
{
    public interface ITooltipInfo
    {
        UnityEngine.Transform transform { get; }
        string TooltipInfo { get; }
    }
}
