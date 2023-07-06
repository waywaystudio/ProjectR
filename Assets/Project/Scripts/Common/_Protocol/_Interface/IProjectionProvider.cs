namespace Common
{
    public interface IProjectionProvider : ISequencerHolder
    {
        float CastingWeight { get; }
        UnityEngine.Vector3 SizeVector { get; }
    }
}
