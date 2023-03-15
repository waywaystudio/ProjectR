namespace Common
{
    public interface IBehaviourCore
    {
        CharacterActionMask BehaviourMask { get; }
        CharacterActionMask IgnorableMask { get; }
        
        // + ActionTable OnCanceled { get; }
        // + ActionTable OnCompleted { get; }
        // + ActionTable OnEnded { get; }
    }

    public interface IBehaviourSequence : IBehaviourCore, ISequence
    {
        // + CharacterActionMask BehaviourMask { get; }
        // + CharacterActionMask IgnorableMask { get; }
        // + ConditionTable Conditions { get; }
        // + ActionTable OnCanceled { get; }
        // + ActionTable OnCompleted { get; }
        // + ActionTable OnEnded { get; }
        
        // + ActionTable OnActivated { get; }
    }
}
