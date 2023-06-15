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
        // [SerializeField] protected SectionTable sectionTable = new();
        
        [SerializeField] protected OldSection activeSection = new();
        [SerializeField] protected OldSection cancelSection = new();
        [SerializeField] protected OldSection completeSection = new();
        [SerializeField] protected OldSection endSection = new();
        // 

        protected CancellationTokenSource Cts;
        
        public bool IsAbleToActive => conditionTable == null || conditionTable.IsAllTrue;
        public bool IsDone { get; protected set; }
        
        public ConditionTable Condition => conditionTable;
        public OldSection ActiveSection => activeSection;
        public OldSection CancelSection => cancelSection;
        public OldSection CompleteSection => completeSection;
        public OldSection EndSection => endSection;

        public async UniTaskVoid ActiveAwait() => await ActiveSection.Invoke();
        
        public void Active(CancellationToken token = default) => activeSection.Invoke(token);
        
        public void Cancel(CancellationToken token = default)
        {
            Cts?.Cancel();
            cancelSection.Invoke(token);
            End();
        }
        
        public void Complete(CancellationToken token = default)
        {
            completeSection.Invoke(token);
            End();
        }

        public void End() => endSection.Invoke();
        
        
        
        public void Add(SectionType sectionType, string key, Action action)
        {
            
        }
        
        public void Remove(SectionType sectionType, string key, Action action)
        {
            
        }


#if UNITY_EDITOR
        [SerializeField] protected GameObject[] EditorExtract;
        
        /// <summary>
        /// EditorExtract에 있는 게임오브젝트에서 Key..methodKey() 방식으로 되어 있는 함수를 자동으로 UnityEvent에 할당한다.
        /// </summary>
        /// <param name="methodKey">ex.Active, Cancel or End and so on</param>
        protected void AddPersistantEvent(GameObject targetObject, string methodKey, OldSection section)
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
        protected void AddPersistantEvent<T>(GameObject targetObject, string methodKey, OldSection<T> section)
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
        public void ActiveSequence()
        {
            StartSequence().Forget();
        }

        public void Clear()
        {
            Condition.Clear();
            
            ActiveSection.Clear();
            CancelSection.Clear();
            CompleteSection.Clear();
            EndSection.Clear();
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
        [SerializeField] private OldSection<T> activeParamSection;
        
        public OldSection<T> ActiveParamSection => activeParamSection;

        public void Active(T value, CancellationToken token = default)
        {
            activeParamSection.Invoke(value, token);
        }
        
        public async UniTaskVoid ActiveAwait(T value, CancellationToken token = default)
        {
            await activeParamSection.Invoke(value, token);
        }
        

        public void ActivateSequence(T value)
        {
            if (Condition.HasFalse) return;
            
            StartSequence(value).Forget();
        }

        public void Clear()
        {
            ActiveParamSection.Clear();
            
            ActiveSection.Clear();
            Condition.Clear();
            CancelSection.Clear();
            CompleteSection.Clear();
            EndSection.Clear();
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
