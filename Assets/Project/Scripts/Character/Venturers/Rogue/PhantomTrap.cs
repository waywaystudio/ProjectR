using System.Collections.Generic;
using Common;
using Common.Animation;
using Common.Traps;
using UnityEngine;

namespace Character.Venturers.Rogue
{
    public class PhantomTrap : TrapComponent
    {
        [SerializeField] private string doubleStabAnimationKey = "AttackDuelFastStabTwice";
        [SerializeField] private string throwAnimationKey = "Cast3";
        [SerializeField] private AnimationModel model;
        
        private readonly Collider[] colliderBuffers = new Collider[32];

        public override void Initialize(ICombatProvider provider)
        {
            base.Initialize(provider);

            SequenceBuilder.Add(SectionType.Active, "IdleAnimation", Idle)
                           .Add(SectionType.Active, "RotateToIdentity", () => transform.rotation = Quaternion.identity)
                           .Add(SectionType.End, "RemoveSkillHit", () => model.OnHit.Remove("SkillHit"));
        }

        public void DoubleStab(Vector3 targetPosition, float timeScale)
        {
            var trapPosition = transform.position;
            var direction = (targetPosition - trapPosition).normalized;
            
            transform.LookAt(targetPosition);

            model.OnHit.Add("SkillHit", () =>
            {
                var takerList = GetTakersInSphereType(trapPosition, 8f, 135f, targetLayer);
                
                takerList?.ForEach(taker => executor.Execute(ExecuteGroup.Group1, taker));
            });
            model.Flip(direction);
            model.Play(doubleStabAnimationKey, 0, false, timeScale, () =>
            {
                Idle();
                model.OnHit.Remove("SkillHit");
            });
        }
        
        public void Throw(Vector3 targetPosition, float timeScale)
        {
            var trapPosition = transform.position;
            var direction = (targetPosition - trapPosition).normalized;
            
            transform.LookAt(targetPosition);

            model.OnHit.Add("SkillHit", () =>
            {
                executor.Execute(ExecuteGroup.Group2, targetPosition);
            });
            model.Flip(direction);
            model.Play(throwAnimationKey, 0, false, timeScale, () =>
            {
                Idle();
                model.OnHit.Remove("SkillHit");
            });
            // Projectile Fire
        }
        
        public override void Execution()
        {
            
        }


        private void Idle() => model.Idle();

        private List<ICombatTaker> GetTakersInSphereType(Vector3 center, float radius, float angle, LayerMask layer)
        {
            if (Physics.OverlapSphereNonAlloc(center, radius, colliderBuffers, layer) == 0) return null;
        
            var result = new List<ICombatTaker>();
            
            colliderBuffers.ForEach(collider =>
            {
                if (collider.IsNullOrEmpty() || !collider.TryGetComponent(out ICombatTaker taker)) return;
                
                if (Mathf.Abs(angle - 360.0f) > 0.000001f)
                {
                    var direction = (collider.transform.position - center).normalized;
        
                    if (Vector3.Angle(transform.forward, direction) > angle * 0.5f) return;
                }
                
                result.Add(taker);
            });
        
            return result;
        }
    }
}
