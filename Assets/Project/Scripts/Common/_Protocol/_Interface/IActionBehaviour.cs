namespace Common
{
    public interface IActionBehaviour
    {
        CharacterActionMask BehaviourMask { get; }

        void Cancel();
    }
}
