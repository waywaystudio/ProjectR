namespace Common
{
    public interface IActionBehaviour
    {
        CharacterActionMask BehaviourMask { get; }
        CharacterActionMask IgnorableMask { get; }

        void Cancel();
    }
}
