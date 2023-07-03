using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Priest.Skills
{
    public class HealingTouch : SkillComponent
    {
        private Vector3 predicatePosition = Vector3.zero;
        
        public override void Initialize()
        {
            base.Initialize();
            
            cost.PayCondition.Add("HasTarget", HasTarget);

            Builder
                .AddActiveParam("SavePredicatePosition", TargetHealing)
                .Add(SectionType.Execute, "PlayCastCompleteAnimation", PlayCastCompleteAnimation)
                .Add(SectionType.Execute, "ExecuteHealingTouch",ExecuteHealingTouch);
        }
        
        
        private void PlayCastCompleteAnimation()
        {
            Cb.Animating.PlayOnce("CastHoldFire", 1f + Haste, Invoker.Complete);
        }

        private void ExecuteHealingTouch()
        {
            var onRapture = Provider.StatusEffectTable.ContainsKey(DataIndex.LightWeaverStatusEffect);
            
            var validTakerList = detector.GetTakersInCircleRange(predicatePosition, 6f, 360f);
            
            validTakerList?.ForEach(taker =>
            {
                executor.ToTaker(taker);

                if (onRapture)
                {
                    executor.ToTaker(taker);
                }
            });
        }
        
        private bool HasTarget()
        {
            // TODO. ExecuteHealingTouch 함수와 중복
            var takers = detector.GetTakersInCircleRange(predicatePosition, 6f, 360f);

            return !takers.IsNullOrEmpty() 
                   && takers[0].Alive.Value;
        }

        private void TargetHealing(Vector3 targetPosition)
        {
            predicatePosition = ValidPosition(targetPosition);
        }
        
        private Vector3 ValidPosition(Vector3 targetPosition)
        {
            var playerPosition = Cb.transform.position;
            
            if (Vector3.Distance(playerPosition, targetPosition) <= Range)
            {
                return targetPosition;
            }

            var direction = (targetPosition - playerPosition).normalized;
            var destination = playerPosition + direction * Range;

            return destination;
        }
    }
}
