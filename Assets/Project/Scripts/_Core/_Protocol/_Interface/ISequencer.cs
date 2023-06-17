using UnityEngine;

public interface ISequencer
{
    Sequencer Sequencer { get; }
}

public interface ISequencer<T>
{
    Sequencer<T> Sequencer { get; }
}

public interface ISections
{
    ConditionTable Condition { get; }
    ActionTable ActiveAction { get; }
    ActionTable CancelAction { get; }
    ActionTable CompleteAction { get; }
    ActionTable EndAction { get; }
}

public interface IParamSection<T>
{
    ActionTable<T> ActiveParamAction { get; }
}

public interface ISections<T> : ISections
{
    // + ConditionTable Condition
    // + ActionTable ActiveAction
    // + ActionTable CancelAction
    // + ActionTable CompleteAction
    // + ActionTable EndAction
    
    ActionTable<T> ActiveParamAction { get; }
}

public interface IProjectorSections : ISections
{
    // + ConditionTable Condition
    // + ActionTable ActiveAction
    // + ActionTable CancelAction
    // + ActionTable CompleteAction
    // + ActionTable EndAction
    
    float CastWeightTime { get; }
    Vector2 SizeVector { get; }
}
