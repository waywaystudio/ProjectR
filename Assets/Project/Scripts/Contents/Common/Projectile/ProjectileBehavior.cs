using Core;
using UnityEngine;

namespace Common.Projectile
{
    public class ProjectileBehavior : MonoBehaviour
    {
        public ICombatProvider Provider { get; set; }
        public ICombatTaker Taker { get; set; }

        public void Fire()
        {
            
        }
    }
}
