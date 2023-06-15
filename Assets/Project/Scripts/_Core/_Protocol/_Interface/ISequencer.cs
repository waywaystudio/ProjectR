using UnityEngine;

public interface ISequencer
{
    ConditionTable Condition { get; }
    ActionTable ActiveAction { get; }
    ActionTable CancelAction { get; }
    ActionTable CompleteAction { get; }
    ActionTable EndAction { get; }
}

public interface ISequencer<T> : ISequencer
{
    // + ConditionTable Condition
    // + ActionTable ActiveAction
    // + ActionTable CancelAction
    // + ActionTable CompleteAction
    // + ActionTable EndAction
    
    ActionTable<T> ActiveParamAction { get; }
}

public interface IProjectorSequence : ISequencer
{
    // + ConditionTable Condition
    // + ActionTable ActiveAction
    // + ActionTable CancelAction
    // + ActionTable CompleteAction
    // + ActionTable EndAction
    
    float CastingTime { get; }
    Vector2 SizeVector { get; }
}
