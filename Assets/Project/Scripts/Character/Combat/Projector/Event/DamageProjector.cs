using System;
using Core;
using UnityEngine;

namespace Character.Combat.Projector.Event
{
    public class DamageProjector : ProjectorEvent, ICombatEntity
    {
        [SerializeField] private PowerValue damageValue;
        
        private const int MaxBuffer = 50;
        private readonly Collider[] buffer = new Collider[MaxBuffer];
        
        // 이것도....모양에 따라서 다른데... ㅋ
        private float sizeValue;
        private LayerMask targetLayer;

        public DataIndex ActionCode { get; private set; }
        
        public IDynamicStatEntry DynamicStatEntry => Provider.DynamicStatEntry;
        public StatTable StatTable { get; } = new();
        public PowerValue DamageValue { get => damageValue; set => damageValue = value; }


        public void Invoke(ProjectorShapeType shapeType)
        {
            SetColliderBuffer(shapeType);
            
            buffer.ForEach(x =>
            {
                if (x.IsNullOrEmpty()) return;
    
                Debug.Log($"Colliding on {x.name} object");
                x.TryGetComponent(out ICombatTaker target);
                
                target.TakeDamage(this);
            });   
        }
        
        
        private void SetColliderBuffer(ProjectorShapeType shapeType)
        {
            switch (shapeType)
            {
                case ProjectorShapeType.None: break;
                case ProjectorShapeType.Sphere:
                {
                    Physics.OverlapSphereNonAlloc(Taker.Object.transform.position, sizeValue, buffer, targetLayer);
                    break;
                }
                case ProjectorShapeType.Rectangle: break;
                case ProjectorShapeType.ExtendedRectangle: break;
                case ProjectorShapeType.Cone: break;
                default: throw new ArgumentOutOfRangeException(nameof(shapeType), shapeType, null);
            }
        }

        private void Start()
        {
            StatTable.Register(ActionCode, DamageValue);
            StatTable.UnionWith(Provider.StatTable);
        }
    }
}
