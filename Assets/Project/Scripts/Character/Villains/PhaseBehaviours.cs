using System.Threading;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Villains
{
    public class PhaseBehaviours : MonoBehaviour
    {
        [SerializeField] private PhaseSequencer phase1;
        [SerializeField] private PhaseSequencer phase2;

        public PhaseSequencer CurrentPhase { get; private set; }
        
        private CancellationTokenSource cts; 
        private VillainBehaviour vb;
        private VillainBehaviour Vb => vb ??= GetComponentInParent<VillainBehaviour>();


        public void CheckPhaseBehaviour()
        {
            if (CurrentPhase is null || !CurrentPhase.SequenceInvoker.IsAbleToActive) return;
            
            CurrentPhase.SequenceInvoker.Active();
            CurrentPhase = phase2;
        }


        private void PullPhase2Trigger()
        {
            cts = new CancellationTokenSource();

            AddPhase2Trigger(cts.Token).Forget();
        }

        private async UniTaskVoid AddPhase2Trigger(CancellationToken ct)
        {
            await UniTask.WaitUntil(() => Vb.transform.position == Vector3.zero || Vb.BehaviourMask == ActionMask.Stop, cancellationToken: ct);
            
            if (!ct.IsCancellationRequested)
            {
                phase1.SequenceInvoker.Complete();
            }
        }

        private void TriggerToPhase2Cancel()
        {
            cts?.Cancel();
            cts = null;
        }

        private void Awake() => CurrentPhase = phase1;

        private void OnEnable()
        {
            phase1.Initialize();
            phase1.SequenceBuilder
                  .AddCondition("HpRatio", () => Vb.Hp.Value / Vb.StatTable.MaxHp < 0.7f)
                  .Add(Section.Active,"PullPhase2Trigger", PullPhase2Trigger)
                  .Add(Section.Active, "RunToCenter", () => Vb.Run(Vector3.zero))
                  .Add(Section.Cancel, "TriggerToPhase2Cancel", TriggerToPhase2Cancel);
            
            phase2.Initialize();
        }

        private void OnDisable()
        {
            phase1.Clear();
            phase2.Clear();
            
            TriggerToPhase2Cancel();
        }
    }
}
