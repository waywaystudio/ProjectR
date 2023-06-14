using UnityEngine;

namespace Common.Execution
{
    public class CrowdControlExecutor : ExecuteComponent
    {
        /* Annotation
         * 각 CC별로 Component를 만드는 방법도 있겠으나, 일단은 합쳐서 만들어본다. */
        [SerializeField] private CharacterActionMask ccMask;
        [SerializeField] private float knockBackDistance;
        [SerializeField] private float knockBackDuration = 0.5f;
        [SerializeField] private float stunDuration;


        public override void Execution(ICombatTaker taker)
        {
            if (CrowdControlHas(CharacterActionMask.Dead))
            {
                taker.Dead();
                return;
            }
                
            if (CrowdControlHas(CharacterActionMask.Stun))
                taker.Stun(stunDuration);
            
            if (CrowdControlHas(CharacterActionMask.KnockBack))
                taker.KnockBack(transform.position, knockBackDistance, knockBackDuration);
        }

        private bool CrowdControlHas(CharacterActionMask cc) => (ccMask | cc) == ccMask;
        
        private void OnEnable() { Executor?.ExecutionTable.Add(this); }
        private void OnDisable() { Executor?.ExecutionTable.Remove(this); }
    }
}
