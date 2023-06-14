#if UNITY_EDITOR
using System.Reflection;
#endif

using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sequences
{
    [Serializable]
    public abstract class SequencerCore : ISequencer
    {
        [SerializeField] protected string sequenceKey;
        [SerializeField] protected ConditionTable conditionTable = new();
        [SerializeField] protected Section activeSection = new();
        [SerializeField] protected Section cancelSection = new();
        [SerializeField] protected Section completeSection = new();
        [SerializeField] protected Section endSection = new();
        
        protected CancellationTokenSource Cts;
        
        public bool IsAbleToActive => conditionTable == null || conditionTable.IsAllTrue;
        public bool IsDone { get; protected set; }

        public void Cancel()
        {
            Cts?.Cancel();
            cancelSection.Invoke();
            endSection.Invoke();
        }

        public ConditionTable Condition => conditionTable;
        public Section Activation => activeSection;
        public Section Cancellation => cancelSection;
        public Section Complete => completeSection;
        public Section End => endSection;


#if UNITY_EDITOR
        [SerializeField] protected GameObject[] EditorExtract;
        
        /// <summary>
        /// EditorExtract에 있는 게임오브젝트에서 Key..methodKey() 방식으로 되어 있는 함수를 자동으로 UnityEvent에 할당한다.
        /// </summary>
        /// <param name="methodKey">ex.Active, Cancel or End and so on</param>
        protected void AddPersistantEvent(GameObject targetObject, string methodKey, Section section)
        {
            var behaviours = targetObject.GetComponents<MonoBehaviour>();

            foreach (var behaviour in behaviours)
            {
                var methods = behaviour.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

                if (methods.IsNullOrEmpty()) continue;
                
                methods.ForEach(method =>
                {
                    var methodName = method.Name;

                    if (!methodName.StartsWith(sequenceKey) || !methodName.EndsWith(methodKey)) return;
                    
                    section.PersistantEvent.ClearUnityEventInEditor(methodName);
                    section.PersistantEvent.AddPersistantListenerInEditor(behaviour, methodName);
                });
            }
        }
        
        /// <summary>
        /// EditorExtract에 있는 게임오브젝트에서 Key..methodKey() 방식으로 되어 있는 함수를 자동으로 UnityEvent<T>에 할당한다.
        /// </summary>
        /// <param name="methodKey">ex.Active, Cancel or End and so on</param>
        protected void AddPersistantEvent<T>(GameObject targetObject, string methodKey, Section<T> section)
        {
            var behaviours = targetObject.GetComponents<MonoBehaviour>();

            foreach (var behaviour in behaviours)
            {
                var methods = behaviour.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance);

                if (methods.IsNullOrEmpty()) continue;
                
                methods.ForEach(method =>
                {
                    var methodName = method.Name;

                    if (!methodName.StartsWith(sequenceKey) || !methodName.EndsWith(methodKey)) return;
                    
                    section.PersistantEvent.ClearUnityEventInEditor(methodName);
                    section.PersistantEvent.AddPersistantListenerInEditor(behaviour, methodName);
                });
            }
        }
#endif
    }
    
    [Serializable]
    public class Sequencer : SequencerCore
    {
        public void Active()
        {
            StartSequence().Forget();
        }

        public void Clear()
        {
            Condition.Clear();
            
            Activation.Clear();
            Cancellation.Clear();
            Complete.Clear();
            End.Clear();
        }


        private async UniTaskVoid StartSequence()
        {
            Cts = new CancellationTokenSource();
            
            try
            {
                IsDone = false;
                await Sequence(Cts.Token);
            }
            finally
            {
                IsDone = true;
                Cts.Dispose();
                Cts = null;
            }

            IsDone = true;
        }

        private async UniTask Sequence(CancellationToken cancellationToken)
        {
            await activeSection.Invoke(cancellationToken);
            await completeSection.Invoke(cancellationToken);
            await endSection.Invoke(cancellationToken);
        }


#if UNITY_EDITOR
        public void AssignPersistantEvents()
        {
            EditorExtract.ForEach(AssignPersistantEvent);
        }


        private void AssignPersistantEvent(GameObject targetObject)
        {
            AddPersistantEvent(targetObject, "Active", activeSection);
            AddPersistantEvent(targetObject, "Cancel", cancelSection);
            AddPersistantEvent(targetObject, "Complete", completeSection);
            AddPersistantEvent(targetObject, "End", endSection);
        }
#endif
    }
    

    [Serializable]
    public class Sequencer<T> : SequencerCore, ISequencer<T>
    {
        [SerializeField] private Section<T> activeParamSection;
        
        public Section<T> ActiveParamSection => activeParamSection;

        public void Activate(T value)
        {
            if (Condition.HasFalse) return;
            
            StartSequence(value).Forget();
        }

        public void Clear()
        {
            ActiveParamSection.Clear();
            
            Activation.Clear();
            Condition.Clear();
            Cancellation.Clear();
            Complete.Clear();
            End.Clear();
        }


        private async UniTaskVoid StartSequence(T value)
        {
            Cts = new CancellationTokenSource();
            
            try
            {
                IsDone = false;
                await Sequence(value, Cts.Token);
            }
            finally
            {
                IsDone = true;
                Cts.Dispose();
                Cts = null;
            }
            
            IsDone = true;
        }

        private async UniTask Sequence(T value, CancellationToken cancellationToken)
        {
            await activeSection.Invoke(cancellationToken);
            await activeParamSection.Invoke(value, cancellationToken);
            await completeSection.Invoke(cancellationToken);
            await endSection.Invoke(cancellationToken);
        }
        
        
#if UNITY_EDITOR
        public void AssignPersistantEvents()
        {
            EditorExtract.ForEach(AssignPersistantEvent);
        }
        
        private void AssignPersistantEvent(GameObject targetObject)
        {
            AddPersistantEvent(targetObject, "ActiveParam", activeParamSection);
            AddPersistantEvent(targetObject, "Active", activeSection);
            AddPersistantEvent(targetObject, "Cancel", cancelSection);
            AddPersistantEvent(targetObject, "Complete", completeSection);
            AddPersistantEvent(targetObject, "End", endSection);
        }
#endif
    }
}
