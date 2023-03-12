using System.Collections;
using Character.Actions;
using Character.Projector;
using Common;
using Common.Systems;
using UnityEngine;

namespace Character.StatusEffect
{
    public class LivingBomb : DamageOverTimeEffect
    {
        [SerializeField] private float radius = 12f;
        [SerializeField] private float stunDuration = 5f;
        [SerializeField] private CollidingSystem collidingSystem;
        [SerializeField] private LayerMask adventurerLayer;
        [SerializeField] private SphereProjector projector;
        [SerializeField] private ValueCompletion bombPower;

        public override void OnOverride() { }
        
        public override void Active(ICombatProvider provider, ICombatTaker taker)
        {
            base.Active(provider, taker);
            
            bombPower.Initialize(provider, ActionCode);
            
            projector.Initialize(0.25f, radius);
            projector.OnActivated.Invoke();
            projector.SetTaker(taker);
        }
        
        public override void Cancel()
        {
            projector.OnCanceled.Invoke();
            
            base.Cancel();
        }
        
        protected override void Complete()
        {
            collidingSystem.TryGetTakersInSphere
            (
                Taker.Object.transform.position,
                radius,
                360f,
                adventurerLayer,
                out var takerList
            );
            
            // TODO. 함수 구조를 수정하여 데미지 주는 주객 순서와 기절의 주객 순서를 통일하면 좋겠다.
            takerList.ForEach(taker =>
            {
                bombPower.Damage(taker);
                taker.CharacterBehaviour.Stun(stunDuration);
            });
            
            projector.OnCompleted.Invoke();
            
            base.Complete();
        }
        
        protected override void End()
        {
            projector.OnEnded.Invoke();
            
            base.End();
        }

        protected override IEnumerator Effectuating()
        {
            var hastedTick = interval * CharacterUtility.GetHasteValue(Provider.StatTable.Haste);

            ProgressTime.Value = duration;

            while (ProgressTime.Value > 0)
            {
                var tickBuffer = hastedTick;

                while (tickBuffer > 0f)
                {
                    ProgressTime.Value -= Time.deltaTime;
                    tickBuffer         -= Time.deltaTime;
                    
                    yield return null;
                }
                
                tickPower.Damage(Taker);
            }

            Complete();
        }
    }
}
