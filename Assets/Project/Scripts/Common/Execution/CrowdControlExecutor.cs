using UnityEngine;

namespace Common.Execution
{
    public class CrowdControlExecutor : ExecuteComponent
    {
        /* Annotation
         * 각 CC별로 Component를 만드는 방법도 있겠으나, 일단은 합쳐서 만들어본다. */
        [SerializeField] private CharacterActionMask ccMask;
        [SerializeField] private float knockBackDistance;
        [SerializeField] private float stunDuration;


        public override void Execution(ICombatTaker taker, float instantMultiplier = 1f)
        {
            if (CrowdControlHas(CharacterActionMask.Dead))
            {
                taker.Dead();
                return;
            }
                
            if (CrowdControlHas(CharacterActionMask.Stun))
                taker.Stun(stunDuration * instantMultiplier);
            
            if (CrowdControlHas(CharacterActionMask.KnockBack))
                taker.KnockBack(transform.position, knockBackDistance * instantMultiplier);
        }

        private bool CrowdControlHas(CharacterActionMask cc) => (ccMask | cc) == ccMask;
        
        private void OnEnable() { Executor?.ExecutionTable.Add(this); }
        private void OnDisable() { Executor?.ExecutionTable.Remove(this); }
    }
}
