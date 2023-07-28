using UnityEngine;

namespace Common
{
    public interface IReward
    {
        int Grade { get; }
        string Title { get; }
        string Description { get; }
        Sprite Icon { get; }
    }
}
