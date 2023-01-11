using System.Collections.Generic;
using Character.Combat.Projector;
using UnityEngine;

namespace Character.Combat.Entities
{
    public class ProjectorEntity : BaseEntity
    {
        [SerializeField] private List<ProjectorObject> projectorPool;
        
        public override bool IsReady => true;

        public void Generate(TargetEntity target)
        {
            var selfPosition = Provider.Object.transform.position;
            var targetPosition = target.Target.Object.transform.position;
            
            // projectorPool[0].Generate(selfPosition, targetPosition);
            // 스킬의 종류마다 Projector Prefab이 있고, 각 프리팹에 Modifier와 CastingTime이
            // Data 단에서 들어갈 예정.
        }
    }
}
