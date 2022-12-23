using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Common.Projectile
{
    public class ChainHealingBehaviour : ProjectileBehaviour
    {
        [SerializeField] private int bounceCount = 3;
        [SerializeField] private float bounceRange = 25f;

        private const int MaxBufferCount = 25;
        private readonly Collider[] colliderBuffer = new Collider[MaxBufferCount];
        private readonly HashSet<ICombatTaker> bounceTargetList = new();

        public override void Initialize(ICombatProvider sender, ICombatTaker taker)
        {
            base.Initialize(sender, taker);
            
            bounceCount--;

            if (bounceCount <= 0)
            {
                bounceTargetList.Clear();
                return;
            }

            bounceTargetList.Add(taker);

            if (TryAddHash(out var newTarget))
            {
                // TrajectoryTweener.onComplete += () => Initialize(newTarget, completeAction, collidedAction);
            }
            
        }
        
        private bool TryAddHash(out ICombatTaker addedTarget)
        {
            var hitCount = Physics.OverlapSphereNonAlloc(transform.position, 
                bounceRange, colliderBuffer, targetLayer);

#if UNITY_EDITOR
            if (hitCount >= MaxBufferCount)
                Debug.LogWarning($"Overflow Collider Max Buffer size : {MaxBufferCount}");          
#endif

            foreach (var t in colliderBuffer)
            {
                if (t.IsNullOrEmpty()) break;
                if (!t.TryGetComponent(out ICombatTaker combatTaker)) break;
                if (!bounceTargetList.Add(combatTaker)) continue;
                
                addedTarget = combatTaker;
                return true;
            }

            addedTarget = null;
            return false;
        }
    }
}
