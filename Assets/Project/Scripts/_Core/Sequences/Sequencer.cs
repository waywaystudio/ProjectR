using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sequences
{
    public abstract class SequencerCore : MonoBehaviour
    {
        [SerializeField] protected string key;
        [SerializeField] protected ConditionTable conditionTable = new();
        [SerializeField] protected CancelSection cancelSection = new();
        [SerializeField] protected CompleteSection completeSection = new();
        [SerializeField] protected EndSection endSection = new();
        
        protected CancellationTokenSource Cts;
        
        public bool IsAbleToActive => conditionTable == null || conditionTable.IsAllTrue;
        public bool IsDone { get; protected set; }

        public void Cancel()
        {
            if (Cts == null) return;
            
            Cts.Cancel();
            cancelSection.Invoke();
            endSection.Invoke();
        }

        public ConditionTable Condition => conditionTable;
        public CancelSection Cancellation => cancelSection;
        public CompleteSection Complete => completeSection;
        public EndSection End => endSection;


#if UNITY_EDITOR
        [SerializeField] protected GameObject[] EditorExtract;
        
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

                    if (!methodName.StartsWith(key) || !methodName.Contains(methodKey)) return;
                    
                    section.PersistantEvent.ClearUnityEventInEditor(methodName);
                    section.PersistantEvent.AddPersistantListenerInEditor(behaviour, methodName);
                });
            }
        }
        
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

                    if (!methodName.StartsWith(key) || !methodName.Contains(methodKey)) return;
                    
                    section.PersistantEvent.ClearUnityEventInEditor(methodName);
                    section.PersistantEvent.AddPersistantListenerInEditor(behaviour, methodName);
                });
            }
        }
#endif
    }
    
    public class Sequencer : SequencerCore, IEditable
    {
        [SerializeField] protected ActiveSection activeSection = new();
        
        public ActiveSection ActiveSection => activeSection;

        public void Active()
        {
            StartAction().Forget();
        }
        
        public void Clear()
        {
            ActiveSection.Clear();
            
            Condition.Clear();
            Cancellation.Clear();
            Complete.Clear();
            End.Clear();
        }


        protected void Initialize()
        {
            IsDone = false;
            
            activeSection.Initialize();
            completeSection.Initialize();
            endSection.Initialize();
            cancelSection.Initialize();
        }
        
        private async UniTaskVoid StartAction()
        {
            Initialize();
            
            Cts = new CancellationTokenSource();
            
            try
            {
                await RunSequence(Cts.Token);
            }
            finally
            {
                IsDone = true;
                Cts.Dispose();
                Cts = null;
            }

            IsDone = true;
        }

        private async UniTask RunSequence(CancellationToken cancellationToken)
        {
            await activeSection.Invoke(cancellationToken);
            await completeSection.Invoke(cancellationToken);
            await endSection.Invoke(cancellationToken);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
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
}
