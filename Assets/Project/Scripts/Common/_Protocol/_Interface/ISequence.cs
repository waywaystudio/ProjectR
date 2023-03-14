using UnityEngine;

namespace Common
{
    public interface ISequence
    {
        ActionTable OnActivated { get; }
        ActionTable OnCanceled { get; }
        ActionTable OnCompleted { get; }
        ActionTable OnEnded { get; }
    }

    /// <summary>
    /// UI Action Bar에 할당할 수 있다.
    /// </summary>
    public interface IAssignable
    {
        Sprite Icon { get; }
        string Description { get; }
        
        void Activate();
        void Release();
    }
}