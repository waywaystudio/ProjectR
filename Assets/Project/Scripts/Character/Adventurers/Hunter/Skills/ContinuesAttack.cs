using Common.Skills;

namespace Character.Adventurers.Hunter.Skills
{
    public class ContinuesAttack : SkillComponent
    {
        public override void MainAttack()
        {
            // TODO. 현재 Test상 HitScan 방식이어서 이렇고, Projectile로 바뀌면 교체해야 함.
            var providerTransform = Cb.transform;
            
            if (!Cb.Colliding.TryGetTakersByRaycast(
                    providerTransform.position,
                    providerTransform.forward,
                    range,
                    16,
                    targetLayer,
                    out var takerList)) return;
            
            takerList.ForEach(Completion);
        }
        

        protected override void Initialize()
        {
            OnCompleted.Register("EndCallback", End);
        }

        protected override void PlayAnimation()
        {
            Cb.Animating.PlayLoop(animationKey);
        }
    }
}
