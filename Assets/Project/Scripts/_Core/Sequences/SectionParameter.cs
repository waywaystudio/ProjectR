using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Sequences
{
    [Serializable]
    public class Section<T>
    {
        [SerializeField] private UnityEvent<T> persistantEvent;

        public bool IsDone { get; private set; }
        public UnityEvent<T> PersistantEvent => persistantEvent;
        private AwaitTable<T> AwaitEvent { get; } = new();
        
        public UniTask Invoke(T param, CancellationToken token = default) 
            => InvokeAndWait(param, token);


        /// <summary>
        /// otherTable을 값 방식으로 등록.
        /// 함수 실행 시점에 otherTable의 Action만 등록한다.
        /// </summary>
        public void Add(string key, Action<T> action) => AwaitEvent.Add(key, action);
        public void AddAwait(string key, Func<UniTask> action) => AwaitEvent.AddAwait(key, action);
        public void AddAwait(string key, Func<T, UniTask> action) => AwaitEvent.AddAwait(key, action);
        

        /// <summary>
        /// otherTable을 참조 방식 등록.
        /// otherTable의 내용이 변경되면, 변경된 내용을 포함하여 실행한다.
        /// </summary>
        public void Register(string key, ActionTable<T> actionTable) => AwaitEvent.Register(key, actionTable);
        public void Register(string key, AwaitTable<T> actionTable) => AwaitEvent.Register(key, actionTable);
        public void Initialize() { IsDone = false; }
        public void Done() => IsDone = true;
        public void Cancel() => AwaitEvent.Cancel();
        public void Remove(string key) => AwaitEvent.Remove(key);
        public void Clear() => AwaitEvent.Clear();
        

        private async UniTask InvokeAndWait(T param, CancellationToken cancellationToken)
        {
            persistantEvent?.Invoke(param);
            await AwaitEvent.Invoke(param, cancellationToken);

            IsDone = true;
        }
    }
}
