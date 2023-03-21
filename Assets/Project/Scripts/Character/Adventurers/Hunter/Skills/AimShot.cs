using Common.Skills;

namespace Character.Adventurers.Hunter.Skills
{
    public class AimShot : SkillComponent
    {
        public override void Execution()
        {
            // TODO. 현재 Test상 HitScan 방식이어서 이렇고, Projectile로 바뀌면 교체해야 함.
            ExecutionTable.Execute(null);
            
            // if (!TryGetTakersByRayCast(out var takerList)) return;
            // takerList.ForEach(ExecutionTable.Execute);
        }
        

        protected override void Initialize()
        {
            OnCompleted.Register("Execution", Execution);
            OnCompleted.Register("PlayEndChargingAnimation", PlayEndChargingAnimation);
        }
        
        protected override void PlayAnimation()
        {
            Cb.Animating.PlayOnce(animationKey);
        }

        private void PlayEndChargingAnimation()
        {
            Cb.Animating.PlayOnce("heavyAttack", 0f, End);
        }
    }
}
