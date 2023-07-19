using UnityEngine;

namespace Common
{
    public interface IProjection : ICombatSequence
    {
        float CastingTime { get; }
        SizeEntity SizeEntity { get; }
    }
}
