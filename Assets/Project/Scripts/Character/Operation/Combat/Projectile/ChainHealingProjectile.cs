using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;

namespace Common.Character.Operation.Combat.Projectile
{
    public class ChainHealingProjectile : ProjectileBehaviour
    {
        [SerializeField] private int bounceCount = 3;
        [SerializeField] private float bounceRange = 25f;
        
        private const int MaxBufferCount = 25;
        private readonly Collider[] colliderBuffer = new Collider[MaxBufferCount];
        private readonly HashSet<ICombatTaker> bounceTargetList = new();
        
        protected override void Trajectory()
        {
            TrajectoryTweener = transform
                                .DOMove(Destination, speed)
                                .SetEase(Ease.Linear)
                                .OnComplete(Arrived)
                                .SetSpeedBased();
            
            TrajectoryTweener.OnUpdate(() =>
            {
                var takerPosition = Taker.Object.transform.position;
                
                if (Vector3.Distance(transform.position, takerPosition) > 1f)
                {
                    TrajectoryTweener.ChangeEndValue(takerPosition, speed, true)
                                     .SetSpeedBased();
                }
            });
        }

        protected void Arrived()
        {
            if (ValidateTaker) Taker.TakeHeal(HealEntity);
            
            if (--bounceCount <= 0)
            {
                bounceTargetList.Clear();
                Destroy(gameObject, 0.1f);
                return;
            }
            
            bounceTargetList.Add(Taker);
            
            if (TryAddHash(out var newTarget)) 
                Initialize(Sender, newTarget);
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
