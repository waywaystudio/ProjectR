using UnityEngine;

namespace Common
{
    public interface ICharacterAction
    {
        CharacterActionMask ActionType { get; }
        CharacterActionMask DisableActionMask { get; }
        
        ConditionTable Conditions { get; }
    }

    public interface IActionBehaviour
    {
        // void Run(Vector3 destination);
        // void Rotate(Vector3 targetPosition);
        // void Stop();
        // void Stun(float duration);
        // void KnockBack(Vector3 source, float distance);
        // void Dead();
        // void Horror(float duration);
    }
}