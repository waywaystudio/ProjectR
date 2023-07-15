using UnityEngine;

namespace Common
{
    public interface IProjection : ICombatSequence
    {
        float CastingTime { get; }
        Vector3 SizeVector { get; }
    }
}
