using Sequences;

public interface ISequencer
{
    Sequencer Sequencer { get; }
}

public interface ISequencer<T>
{
    Sequencer<T> Sequencer { get; }
}
