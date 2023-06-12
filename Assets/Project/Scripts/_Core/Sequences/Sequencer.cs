using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Sequences
{
    public class Sequencer : MonoBehaviour
    {
        [SerializeField] private Section activeSection;
        [SerializeField] private Section completeSection;
        [SerializeField] private Section endSection;
        [SerializeField] private Section cancelSection;
        
        private CancellationTokenSource cts;

        public void Activate()
        {
            StartAction().Forget();
        }
        
        public void Cancel()
        {
            if (cts == null) return;
            
            cts.Cancel();
            cancelSection.Invoke();
            endSection.Invoke();
        }
        
        
        private async UniTaskVoid StartAction()
        {
            SectionInitialize();
            
            cts = new CancellationTokenSource();
            
            try
            {
                await RunSequence(cts.Token);
            }
            finally
            {
                cts.Dispose();
                cts = null;
            }
        }

        private async UniTask RunSequence(CancellationToken cancellationToken)
        {
            activeSection.Invoke(cancellationToken).Forget();
            await UniTask.WaitUntil(() => activeSection.IsDone, cancellationToken: cancellationToken);

            completeSection.Invoke(cancellationToken).Forget();
            await UniTask.WaitUntil(() => completeSection.IsDone, cancellationToken: cancellationToken);

            endSection.Invoke(cancellationToken).Forget();
            await UniTask.WaitUntil(() => endSection.IsDone, cancellationToken: cancellationToken);
        }

        private void SectionInitialize()
        {
            activeSection.Initialize();
            completeSection.Initialize();
            endSection.Initialize();
            cancelSection.Initialize();
        }
    }
}
