namespace Common
{
    public interface IProjectionProvider : ICombatSequence
    {
        float CastingWeight { get; }
        UnityEngine.Vector3 SizeVector { get; }
    }
}
