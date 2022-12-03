using Core;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class TargetEntity : EntityAttribution, ITargetEntity, IReadyRequired
    {
        [SerializeField] private int targetCount;
        [SerializeField] private LayerMask targetLayer;

        private ICombatTaker target;

        public int TargetCount { get => targetCount; set => targetCount = value; }
        public LayerMask TargetLayer { get =>  targetLayer; set => targetLayer = value; }

        public ICombatTaker Target
        {
            get => target;
            set
            {
                if (!value.TargetObject.IsInLayerMask(TargetLayer)) return;
                target = value;
            }
        }

        public bool IsReady => Target != null;
        
        private void Awake()
        {
            Flag = EntityType.Target;
        }

#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            base.OnEditorInitialize();
            
            Flag = EntityType.Target;
            TargetCount = StaticData.TargetCount;
        }
#endif
        
    }
}
