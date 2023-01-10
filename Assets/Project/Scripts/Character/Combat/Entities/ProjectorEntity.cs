using System.Collections.Generic;
using Character.Combat.Projector;
using UnityEngine;

namespace Character.Combat.Entities
{
    public class ProjectorEntity : BaseEntity
    {
        [SerializeField] private List<ProjectorObject> projectorPool;
        
        public override bool IsReady => true;

        public void Generate(TargetEntity target, CastingEntity casting, float modifier)
        {
            
        }
    }
}
