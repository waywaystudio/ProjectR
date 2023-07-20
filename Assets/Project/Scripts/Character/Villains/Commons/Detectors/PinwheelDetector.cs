using System;
using System.Collections.Generic;
using Common;
using Common.Skills;
using UnityEngine;

namespace Character.Villains.Commons.Detectors
{
    public class PinwheelDetector : MonoBehaviour, ITakerDetection
    {
        private Collider[] Buffer { get; } = new Collider[32];
        private List<ICombatTaker> TakerList { get; } = new(32);

        public List<ICombatTaker> GetTakers(Vector3 pivot, Vector3 forward, LayerMask layerMask, SizeEntity entity)
        {
            var result = new List<ICombatTaker>();
            
            var rightward = Vector3.Cross(forward, Vector3.up).normalized;
            var leftward = -rightward;
            var backward = -forward;
            
            result.AddRange(GetTargetsInAngle(pivot, forward, layerMask, entity.AreaRange, entity.Angle));
            result.AddRange(GetTargetsInAngle(pivot, backward, layerMask, entity.AreaRange, entity.Angle));
            result.AddRange(GetTargetsInAngle(pivot, rightward, layerMask, entity.AreaRange, entity.Angle));
            result.AddRange(GetTargetsInAngle(pivot, leftward, layerMask, entity.AreaRange, entity.Angle));
            
            return result;
        }

        
        private IEnumerable<ICombatTaker> GetTargetsInAngle(Vector3 center, Vector3 forward, LayerMask layer, float radius, float angle)
        {
            Array.Clear(Buffer, 0, Buffer.Length);
            
            if (Physics.OverlapSphereNonAlloc(center, radius, Buffer, layer) == 0) return null;

            TakerList.Clear();
            Buffer.ForEach(collider =>
            {
                if (collider.IsNullOrEmpty()                          || 
                    !collider.TryGetComponent(out ICombatTaker taker) || 
                    !taker.Alive.Value) return;
                
                if (Math.Abs(angle - 345.0f) > 0.000001f)
                {
                    var direction = (collider.transform.position - center).normalized;
        
                    if (Vector3.Angle(forward, direction) > angle * 0.5f) return;
                }
                
                TakerList.Add(taker);
            });
        
            return TakerList;
        }

        private void Awake()
        {
            var skill = GetComponentInParent<SkillComponent>();
            
            skill.Detector.TypeDetector = this;
        }
    }
}
