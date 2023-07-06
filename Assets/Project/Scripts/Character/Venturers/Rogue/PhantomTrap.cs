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

            SequenceBuilder.Add(Section.Active, "IdleAnimation", Idle)
                           .Add(Section.Active, "RotateToIdentity", () => transform.rotation = Quaternion.identity)
                           .Add(Section.End, "RemoveSkillHit", () => model.OnHit.Remove("SkillHit"));
        }

        public void DoubleStab(Vector3 targetPosition, float timeScale)
        {
            var trapPosition = transform.position;
            var direction = (targetPosition - trapPosition).normalized;
            
            transform.LookAt(targetPosition);

            model.OnHit.Add("SkillHit", () =>
            {
                var takerList = 
                    TargetUtility.GetTargetsInAngle<ICombatTaker>(trapPosition, transform.forward, targetLayer, 5f, 135f, colliderBuffers);
                
                takerList?.ForEach(taker => executor.ToTaker(taker, ExecuteGroup.Group1));
            });
            model.Flip(direction);
            model.Play(doubleStabAnimationKey, 0, false, timeScale, () =>
            {
                Idle();
                model.OnHit.Remove("SkillHit");
            });
        }
        
        public void MarkOfDeath(Vector3 targetPosition, float timeScale)
        {
            var trapPosition = transform.position;
            var direction = (targetPosition - trapPosition).normalized;
            
            transform.LookAt(targetPosition);

            model.OnHit.Add("SkillHit", () =>
            {
                executor.ToPosition(targetPosition, ExecuteGroup.Group2);
            });
            model.Flip(direction);
            model.Play(throwAnimationKey, 0, false, timeScale, () =>
            {
                Idle();
                model.OnHit.Remove("SkillHit");
            });
        }
        
        public override void Execution() { }
        

        private void Idle() => model.Idle();
    }
}
