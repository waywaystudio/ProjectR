using UnityEngine;

namespace Core
{
    public interface IPathfinding
    {
        bool IsReached { get; }
        Vector3 Direction { get; }
    }
}
