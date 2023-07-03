using Common;
using UnityEngine;

namespace Character.Villains
{
    public class PhaseBehaviours : MonoBehaviour
    {
        [SerializeField] private PhaseSequencer phase1;
        [SerializeField] private PhaseSequencer phase2;

        public PhaseSequencer CurrentPhase { get; private set; }
        
        private VillainBehaviour vb;
        private VillainBehaviour Vb => vb ??= GetComponentInParent<VillainBehaviour>();


        public void CheckPhaseBehaviour()
        {
            if (CurrentPhase is null || !CurrentPhase.SequenceInvoker.IsAbleToActive) return;
            
            CurrentPhase.SequenceInvoker.Active();
            CurrentPhase = phase2;
        }


        private void Awake() => CurrentPhase = phase1;

        private void OnEnable()
        {
            phase1.Initialize();
            phase1.SequenceBuilder
                  .AddCondition("HpRatio", () => Vb.Hp.Value / Vb.StatTable.MaxHp < 0.7f)
                  .AddTrigger(() => Vb.transform.position == Vector3.zero || Vb.BehaviourMask == ActionMask.Stop, phase1.SequenceInvoker.Complete)
                  .Add(SectionType.Active, "RunToCenter", () => Vb.Run(Vector3.zero));
            
            phase2.Initialize();
        }

        private void OnDisable()
        {
            phase1.Clear();
            phase2.Clear();
        }
    }
}
