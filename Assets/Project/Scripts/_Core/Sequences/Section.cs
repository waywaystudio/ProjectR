using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Sequences
{
    public class Section : MonoBehaviour
    {
        [SerializeField] private bool isInstant;
        [SerializeField] private UnityEvent persistantEvent;

        public bool IsDone { get; private set; }
        private ActionTable DynamicEvent { get; } = new();

        public UniTask Invoke(CancellationToken token = default) 
            => InvokeAndWait(token);
        
        public void Initialize() { IsDone = isInstant; }

        /// <summary>
        /// otherTable을 값 방식으로 등록.
        /// 함수 실행 시점에 otherTable의 Action만 등록한다.
        /// </summary>
        public void Add(string key, Action action) => DynamicEvent.Register(key, action);

        /// <summary>
        /// otherTable을 값 방식으로 등록.
        /// 함수 실행 시점에 otherTable의 Action만 등록한다.
        /// </summary>
        public void Add(ActionTable actionTable)
        {
            actionTable.ForEach(action => DynamicEvent.Register(action.Key, action.Value));
        }

        /// <summary>
        /// otherTable을 참조 방식 등록.
        /// otherTable의 내용이 변경되면, 변경된 내용을 포함하여 실행한다.
        /// </summary>
        public void Register(string key, ActionTable actionTable) => DynamicEvent.Register(key, actionTable);
        public void Remove(string key) => DynamicEvent.Unregister(key);
        public void Done() => IsDone = true;
        public void Clear() => DynamicEvent.Clear();
        

        private async UniTask InvokeAndWait(CancellationToken cancellationToken)
        {
            persistantEvent?.Invoke();
            DynamicEvent.Invoke();

            await UniTask.WaitUntil(() => IsDone, cancellationToken: cancellationToken);
        }
    }
}