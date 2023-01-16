using UnityEngine;

namespace Core
{
    public interface IPathfinding
    {
        bool IsReached { get; }
        bool IsSafe { get; }
        Vector3 Direction { get; }
    }
}
