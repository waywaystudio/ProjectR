namespace Core
{
    public interface ISequence
    {
        ActionTable OnActivated { get; }
        ActionTable OnCanceled { get; }
        ActionTable OnCompleted { get; }
        ActionTable OnEnded { get; }
    }
}
