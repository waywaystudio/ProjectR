using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class TargetEntity : EntityAttribution, ITargetEntity
    {
        [SerializeField] private int targetCount;
        [SerializeField] private LayerMask targetLayer;

        private ICombatTaker target;
        private List<ICombatTaker> targetList;

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
        
        public List<ICombatTaker> TargetList
        {
            get => targetList;
            set
            {
                var result = value.Where(x => x.TargetObject.IsInLayerMask(TargetLayer))
                                                 .ToList();
                
                var capacity = targetCount >= result.Count
                    ? result.Count
                    : targetCount;

                targetList = result.GetRange(0, capacity);
            }
        }
        
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
