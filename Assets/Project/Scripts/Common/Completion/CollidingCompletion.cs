using UnityEngine;

namespace Common.Completion
{
    public abstract class CollidingCompletion : MonoBehaviour
    {
        [SerializeField] protected DataIndex actionCode;
        
        protected ICombatProvider Provider;
        
        public virtual void Initialize(ICombatProvider provider)
        {
            Provider = provider;
        }

        public abstract void Completion(ICombatTaker taker);
    }
}
