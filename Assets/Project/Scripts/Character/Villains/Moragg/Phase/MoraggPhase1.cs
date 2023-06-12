using System.Collections;
using Common;
using UnityEngine;

namespace Character.Villains.Moragg.Phase
{
    public class MoraggPhase1 : VillainPhase
    {
        [SerializeField] private float nextPhaseHpRatio;

        private float HpRatio => Villain.DynamicStatEntry.Hp.Value / Villain.StatTable.MaxHp; 
        
        private void OnEnable()
        {
            Conditions.Register("HpRatio0.7f", () => HpRatio < nextPhaseHpRatio);
            OnCompleted.Register("ToCenter", () => Villain.Run(Vector3.zero));
            OnCompleted.Register("WaitUntilArrived", () => StartCoroutine(WaitUntilArrived()));
        }

        private void OnDisable()
        {
            Conditions.Unregister("HpRatio0.7f");
            OnCompleted.Unregister("ToCenter");
            OnCompleted.Unregister("WaitUntilArrived");
        }

        private IEnumerator WaitUntilArrived()
        {
            yield return new WaitUntil(() => Villain.BehaviourMask == CharacterActionMask.Stop);
            
            End();
        }
    }
}
