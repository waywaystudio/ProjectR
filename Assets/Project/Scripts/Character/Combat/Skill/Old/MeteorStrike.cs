using System.Collections.Generic;
using UnityEngine;

namespace Character.Combat.Skill
{
    public class MeteorStrike : SkillObject
    {
        [SerializeField] private float randomRadius;
        [SerializeField] private int farRadius;
        [SerializeField] private int count;

        private List<Vector2> sampler;

        protected override void OnAssigned()
        {
            OnCompleted.Register(InstanceID, OnMeteorStrikeCompleted);
        }

        private void OnMeteorStrikeCompleted()
        {
            if (ProjectorModule)
            {
                sampler = ProjectorUtility.GetPointsInCircle((int)randomRadius, farRadius, count, ProjectorUtility.SamplingCount);
                sampler.ForEach(x => ProjectorModule.Projection(new Vector3(x.x, 0f, x.y)));
            }
        }
    }
}
