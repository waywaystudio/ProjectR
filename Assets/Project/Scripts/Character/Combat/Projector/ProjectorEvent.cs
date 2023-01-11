using Core;
using UnityEngine;

namespace Character.Combat.Projector
{
    public abstract class ProjectorEvent : MonoBehaviour
    {
        public ICombatProvider Provider { get; set; }
        public ICombatTaker Taker { get; set; }
        
        public void Initialize(ICombatProvider provider, ICombatTaker taker, float sizeValue, LayerMask targetLayer)
        {
            Provider = provider;
            Taker    = taker;
        }
    }
}
