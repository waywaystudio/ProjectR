using System.Collections.Generic;
using Character.Skill;
using Core;
using UnityEngine;

namespace Character.TargetingSystem
{
    public class SphereColliderTargeting : Targeting
    {
        [SerializeField] private float radius;
        [SerializeField] private float angle;

        public override void Initialize(SkillComponent skill)
        {
            // targetLayer = skill.targetLayer;
            // radius = skill.Range;
            // angle = skill.Angle;
        }

        public override bool TryGetTargetList(Vector3 center, out List<ICombatTaker> result)
        {
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
                    if (Mathf.Abs(angle - 360.0f) > 0.0001f)
                    {
                        var direction = (collider.transform.position - center).normalized;
                    
                        if (Vector3.Angle(transform.forward, direction) <= angle * 0.5f)
                        {
                            takerList.Add(taker);
                        }
                    }
                    else
                    {
                        takerList.Add(taker);
                    }
                });

                result = takerList;
            }

            return result.HasElement();
        }
    }
}
