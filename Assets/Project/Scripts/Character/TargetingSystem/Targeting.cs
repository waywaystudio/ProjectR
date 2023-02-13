using System.Collections.Generic;
using Character.Skill;
using Core;
using UnityEngine;

namespace Character.TargetingSystem
{
    public abstract class Targeting : MonoBehaviour
    {
        [SerializeField] protected LayerMask targetLayer;

        protected readonly Collider[] Buffers = new Collider[32];

        public abstract void Initialize(SkillComponent skill);
        public abstract bool TryGetTargetList(Vector3 center, out List<ICombatTaker> result);
    }
}
