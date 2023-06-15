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
            if (CurrentPhase is null || !CurrentPhase.IsAbleToActive) return;
            
            CurrentPhase.Active();
            CurrentPhase = phase2;
        }


        private void Awake() => CurrentPhase = phase1;

        private void OnEnable()
        {
            phase1.Initialize();
            phase1.Condition.Add("HpRatio", () => Vb.DynamicStatEntry.Hp.Value / Vb.StatTable.MaxHp < 0.7f);
            phase1.ActiveAction.Add("RunToCenter", () => Vb.Run(Vector3.zero));
            phase1.AddCompleteTrigger(() => Vb.transform.position == Vector3.zero || Vb.BehaviourMask == CharacterActionMask.Stop);
            
            phase2.Initialize();
        }

        private void OnDisable()
        {
            phase1.Clear();
            phase2.Clear();
        }
    }
}
