using UnityEngine;

public interface ISequencerHolder
{
    Sequencer Sequencer { get; }
}

public interface ISequencerHolder<T>
{
    Sequencer<T> Sequencer { get; }
}

public interface IProjectorSequencer : ISequencerHolder
{
    float CastWeightTime { get; }
    Vector3 SizeVector { get; }
}

public interface IProjectorSequencer<T> : ISequencerHolder<T>
{
    float CastWeightTime { get; }
    Vector3 SizeVector { get; }
}
