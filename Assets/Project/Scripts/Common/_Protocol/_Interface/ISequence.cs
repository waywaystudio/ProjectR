namespace Common
{
    public interface ISequenceCore
    {
        ActionTable OnCanceled { get; }
        ActionTable OnCompleted { get; }
        ActionTable OnEnded { get; }
    }
    
    public interface ISequence : ISequenceCore
    {
        ActionTable OnActivated { get; }
    }

    public interface ISequence<T> : ISequenceCore
    {
        ActionTable<T> OnActivated { get; }
    }
    
    public interface ISequence<T0, T1> : ISequenceCore
    {
        ActionTable<T0, T1> OnActivated { get; }
    }
}