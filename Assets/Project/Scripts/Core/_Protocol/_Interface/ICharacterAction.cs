using UnityEngine;

namespace Core
{
    public interface ICharacterAction
    {
        CharacterActionMask ActionType { get; }
        CharacterActionMask DisableActionMask { get; }
        
        ConditionTable Conditions { get; }
    }

    public interface ICharacterBehaviour
    {
        void Run(Vector3 destination);
        void Rotate(Vector3 targetPosition);
        void Stop();
        void Stun(float duration);
        void KnockBack(Vector3 source, float distance);
        void Dead();
        // void KnockDown(float duration);
        // void Horror(float duration);
    }
}
