namespace Common
{
    public interface ISequence
    {
        ActionTable OnActivated { get; }
        ActionTable OnCanceled { get; }
        ActionTable OnCompleted { get; }
        ActionTable OnEnded { get; }
    }

    public static class SequenceExtension
    {
        /// <summary>
        /// main에서 Action을 실행할 때, sub의 ActionTable을 참조한다. 
        /// </summary>
        public static void Combine(this ISequence main, string key, ISequence sub)
        {
            main.OnActivated.Register(key, sub.OnActivated);
            main.OnCanceled.Register(key, sub.OnCanceled);
            main.OnCompleted.Register(key, sub.OnCompleted);
            main.OnEnded.Register(key, sub.OnEnded);
        }
        
        /// <summary>
        /// 결합 시점에 sub 시퀀스 ActionTable의 Action만 포함한다. 
        /// </summary>
        public static void Combine(this ISequence main, ISequence sub)
        {
            main.OnActivated.Register(sub.OnActivated);
            main.OnCanceled.Register(sub.OnCanceled);
            main.OnCompleted.Register(sub.OnCompleted);
            main.OnEnded.Register(sub.OnEnded);
        }

        public static void Clear(this ISequence sequence)
        {
            sequence.OnActivated.Clear();
            sequence.OnCanceled.Clear();
            sequence.OnCompleted.Clear();
            sequence.OnEnded.Clear();
        }
    }
}