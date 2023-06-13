using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Character.Villains
{
    public class PhaseBehaviours : MonoBehaviour
    {
        [SerializeField] private PhaseSequencer phase1Seq;
        [SerializeField] private PhaseSequencer phase2Seq;
        
        public PhaseSequencer CurrentPhase { get; private set; }
        private VillainBehaviour Vb { get; set; }


        public void Initialize(VillainBehaviour vb)
        {
            CurrentPhase = phase1Seq;
            Vb           = vb;
            
            phase1Seq.Condition.Add("HpRatio", Phase1ConditionHpRatio);
            phase1Seq.ActiveSection.AddAwait("WaitUntilArrived", WaitUntilArrived);
        }

        public void CheckPhaseBehaviour()
        {
            if (CurrentPhase is null || !CurrentPhase.IsAbleToActive) return;
            
            CurrentPhase.Active();
            CurrentPhase = phase2Seq;
        }
        
        public bool Phase1ConditionHpRatio()
        {
            var hpRatio = Vb.DynamicStatEntry.Hp.Value / Vb.StatTable.MaxHp;

            return hpRatio < 0.7f;
        }

        public void Phase1ActiveRunToCenter()
        {
            Vb.Run(Vector3.zero);
        }

        public async UniTask WaitUntilArrived() 
            => await UniTask.WaitUntil(() => Vb.BehaviourMask == CharacterActionMask.Stop);
    }
}
