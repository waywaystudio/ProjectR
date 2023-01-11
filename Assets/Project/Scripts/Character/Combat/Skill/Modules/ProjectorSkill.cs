using System.Collections.Generic;
using Character.Combat.Projector;
using Core;
using UnityEngine;

namespace Character.Combat.Skill.Modules
{
    public class ProjectorSkill : SkillModule
    {
        [SerializeField] private List<ProjectorObject> projectorPool;

        public void Generate(ICombatTaker target)
        {
            var selfPosition = Provider.Object.transform.position;
            var targetPosition = target.Object.transform.position;
            
            // projectorPool[0].Generate(selfPosition, targetPosition);
            // 스킬의 종류마다 Projector Prefab이 있고, 각 프리팹에 Modifier와 CastingTime이
            // Data 단에서 들어갈 예정.
        }
    }
}
