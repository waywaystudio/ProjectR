using UnityEngine;

public interface IHasSequencer
{
    Sequencer Sequencer { get; }
}

public interface IHasSequencer<T>
{
    Sequencer<T> Sequencer { get; }
}

public interface IProjectorSequencer : IHasSequencer
{
    float CastWeightTime { get; }
    Vector2 SizeVector { get; }
}
