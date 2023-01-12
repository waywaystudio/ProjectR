using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Character.Combat
{
    using Projector;
    
    public class ProjectorModule : Module
    {
        [SerializeField] private List<ProjectorObject> projectorPool;

        public void Projection(ICombatTaker target)
        {
            if (projectorPool.HasElement())
                projectorPool[0].Projection(Provider, target);
        }

#if UNITY_EDITOR
        public void SetUpValue(float temp)
        {
            
        }
#endif
    }
}
