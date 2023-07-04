namespace Common
{
    public interface IProjectionProvider : IHasSequencer
    {
        float CastingWeight { get; }
        UnityEngine.Vector3 SizeVector { get; }
    }
}
