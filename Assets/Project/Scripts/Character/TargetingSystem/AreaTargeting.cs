using System.Collections.Generic;
using Character.Skill;
using Core;
using UnityEngine;

namespace Character.TargetingSystem
{
    public class AreaTargeting : Targeting
    {
        [SerializeField] private float range;
        [SerializeField] private float radius;
        
        // TODO. Projector
        // TODO. Out of Range
        
        public override void Initialize(SkillComponent skill)
        {
            // targetLayer = skill.targetLayer;
            // radius = skill.Range;
            // angle = skill.Angle;
        }
        
        public override bool TryGetTargetList(Vector3 center, out List<ICombatTaker> result)
        {
            var distance = Vector3.Distance(transform.position, center);
            
            if (distance > range)
            {
                Debug.LogWarning($"Out of Limit:{range}, Current{distance}");
                
                result = null;
                return false;
            }
            
            var targetCount = Physics.OverlapSphereNonAlloc(center, radius, Buffers, targetLayer);

            if (targetCount == 0)
            {
                result = null;
            }
            else
            {
                var takerList = new List<ICombatTaker>();
            
                Buffers.ForEach(collider =>
                {
                    if (collider.IsNullOrEmpty()) return;
                    if (!collider.TryGetComponent(out ICombatTaker taker)) return;
                
                    takerList.Add(taker);
                });

                result = takerList;
            }

            return result.HasElement();
        }
    }
}
