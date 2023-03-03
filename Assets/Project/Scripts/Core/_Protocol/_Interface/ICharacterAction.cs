namespace Core
{
    public interface ICharacterAction
    {
        CharacterActionMask ActionType { get; }
        CharacterActionMask DisableActionMask { get; }
        
        ConditionTable Conditions { get; }
    }
}
