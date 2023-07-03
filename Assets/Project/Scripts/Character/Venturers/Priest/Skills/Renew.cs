using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Venturers.Priest.Skills
{
    public class Renew : SkillComponent
    {
        private Vector3 predicatePosition = Vector3.zero;
        
        public override void Initialize()
        {
            base.Initialize();

            Builder
                .AddActiveParam("SetTargetPosition", SetTargetPosition)
                .Add(SectionType.Execute, "ExecuteRenew", ExecuteRenew);
        }


        private void SetTargetPosition(Vector3 targetPosition)
        {
            predicatePosition = ValidPosition(targetPosition);
        }
        
        private void ExecuteRenew()
        {
            // Get Nearest Single Taker from Clicked Position
            var validTakerList = detector.GetTakersInCircleRange(predicatePosition, 6f, 360f);

            if (validTakerList.IsNullOrEmpty()) return;

            ICombatTaker nearestTaker = null;
            var nearestDistance = float.MaxValue;
            
            validTakerList.ForEach(taker =>
            {
                var distance = Vector3.Distance(taker.Position, Provider.Position);

                if (!(distance < nearestDistance)) return;
                
                nearestDistance = distance;
                nearestTaker    = taker;
            });

            executor.ToTaker(nearestTaker);
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
