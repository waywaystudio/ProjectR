using UnityEngine;

public interface IHasSequencer
{
    Sequencer Sequencer { get; }
}

public interface IHasSequencer<T>
{
    Sequencer<T> Sequencer { get; }
}

// public interface ISections
// {
//     Table<SectionType, Section> Table { get; }
// }
// public interface IParamSection<T>
// {
//     ActionTable<T> ActiveParamAction { get; }
// }
// public interface ISections<T> : ISections, IParamSection<T>
// {
//     // + ConditionTable Condition
//     // + ActionTable ActiveAction
//     // + ActionTable CancelAction
//     // + ActionTable CompleteAction
//     // + ActionTable EndAction
// }

public interface IProjectorSequencer : IHasSequencer
{
    float CastWeightTime { get; }
    Vector2 SizeVector { get; }
}
