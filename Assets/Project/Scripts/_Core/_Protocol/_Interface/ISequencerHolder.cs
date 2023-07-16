using UnityEngine;

public interface ISequencerHolder
{
    Sequencer Sequencer { get; }
}

public interface ISequencerHolder<T>
{
    Sequencer<T> Sequencer { get; }
}