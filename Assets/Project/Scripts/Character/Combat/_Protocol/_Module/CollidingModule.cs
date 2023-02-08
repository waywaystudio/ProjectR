using System;
using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Combat
{
    public class CollidingModule : CombatModule
    {
        [SerializeField] private float range;
        [SerializeField] private float angle;
        [SerializeField] private LayerMask targetLayer;

        private readonly Collider[] buffer = new Collider[16];

        public override void Initialize(CombatComponent combatComponent)
        {
            
        }

        public bool TryGetTargetList(out List<ICombatTaker> targetList)
        {
            var result = new List<ICombatTaker>();
            var characterPosition = transform.position;

            Physics.OverlapSphereNonAlloc(characterPosition, range, buffer, targetLayer);
            
            buffer.ForEach(x =>
            {
                if (x.IsNullOrEmpty()) return;
                if (!x.TryGetComponent(out ICombatTaker taker)) return;
                if (Math.Abs(angle - 360.0f) > 0.0001f)
                {
                    var direction = (x.transform.position - characterPosition).normalized;
                    
                    if (Vector3.Angle(transform.forward, direction) <= angle * 0.5f)
                    {
                        result.Add(taker);
                    }
                }
                else
                {
                    result.Add(taker);
                }
            });

            targetList = result;

            return targetList.HasElement();
        }
    }
}
