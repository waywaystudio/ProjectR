using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Sequences
{
    [Serializable]
    public class Section
    {
        [SerializeField] protected UnityEvent persistantEvent;

        public bool IsDone { get; private set; }
        public UnityEvent PersistantEvent => persistantEvent;
        private AwaitTable AwaitEvent { get; } = new();
        

        public UniTask Invoke(CancellationToken token = default) 
            => InvokeAndWait(token);
        

        /// <summary>
        /// otherTable을 값 방식으로 등록.
        /// 함수 실행 시점에 otherTable의 Action만 등록한다.
        /// </summary>
        public void Add(string key, Action action) => AwaitEvent.Add(key, action);
        public void AddAwait(string key, Func<UniTask> action) => AwaitEvent.AddAwait(key, action);

        /// <summary>
        /// otherTable을 참조 방식 등록.
        /// otherTable의 내용이 변경되면, 변경된 내용을 포함하여 실행한다.
        /// </summary>
        public void Register(string key, AwaitTable actionTable) => AwaitEvent.Register(key, actionTable);
        
        public void Initialize() { IsDone = false; }
        public void Done() => IsDone = true;
        public void Cancel() => AwaitEvent.Cancel();
        public void Remove(string key) => AwaitEvent.Remove(key);
        public void Clear() => AwaitEvent.Clear();
        

        private async UniTask InvokeAndWait(CancellationToken cancellationToken)
        {
            if (persistantEvent.IsNullOrDestroyed() && AwaitEvent.IsNullOrEmpty()) return;
            
            persistantEvent?.Invoke();
            await AwaitEvent.Invoke(cancellationToken);

            IsDone = true;
        }
    }
}