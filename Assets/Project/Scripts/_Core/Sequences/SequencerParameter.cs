using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sequences
{
    public class Sequencer<T> : SequencerCore, IEditable
    {
        [SerializeField] private ActiveSection<T> activeSection;
        
        public ActiveSection<T> ActiveSection => activeSection;

        public void Activate(T value)
        {
            if (Condition.HasFalse) return;
            
            StartAction(value).Forget();
        }

        public void Clear()
        {
            ActiveSection.Clear();
            
            Condition.Clear();
            Cancellation.Clear();
            Complete.Clear();
            End.Clear();
        }
        

        private void SectionInitialize()
        {
            IsDone = false;
            
            activeSection.Initialize();
            completeSection.Initialize();
            endSection.Initialize();
            cancelSection.Initialize();
        }

        private async UniTaskVoid StartAction(T value)
        {
            SectionInitialize();
            
            Cts = new CancellationTokenSource();
            
            try
            {
                await RunSequence(value, Cts.Token);
            }
            finally
            {
                IsDone = true;
                Cts.Dispose();
                Cts = null;
            }
            
            IsDone = true;
        }

        private async UniTask RunSequence(T value, CancellationToken cancellationToken)
        {
            await activeSection.Invoke(value, cancellationToken);
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
