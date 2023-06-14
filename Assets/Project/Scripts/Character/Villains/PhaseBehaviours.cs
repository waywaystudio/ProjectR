using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Villains
{
    public class PhaseBehaviours : MonoBehaviour, IEditable
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

        public void Phase1ActiveRunToCenter()
        {
            Vb.Run(Vector3.zero);
        }
        
        
        private bool Phase1ConditionHpRatio()
        {
            var hpRatio = Vb.DynamicStatEntry.Hp.Value / Vb.StatTable.MaxHp;

            return hpRatio < 0.7f;
        }

        private async UniTask WaitUntilArrived() 
            => await UniTask.WaitUntil(() => Vb.BehaviourMask == CharacterActionMask.Stop);

        private void Awake()
        {
            CurrentPhase = phase1;
            
            phase1.Condition.Add("HpRatio", Phase1ConditionHpRatio);
            phase1.Activation.AddAwait("WaitUntilArrived", WaitUntilArrived);
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            phase1.AssignPersistantEvents();
            phase2.AssignPersistantEvents();
        }
#endif
    }
}
