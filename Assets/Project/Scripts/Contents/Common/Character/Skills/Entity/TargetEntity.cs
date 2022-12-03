using System.Collections.Generic;
using Core;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class TargetEntity : EntityAttribution, ITargetEntity, IReadyRequired
    {
        [SerializeField] private int targetCount;
        [SerializeField] private LayerMask targetLayer;

        public int TargetCount { get => targetCount; set => targetCount = value; }
        public LayerMask TargetLayer { get =>  targetLayer; set => targetLayer = value; }
        public List<ICombatTaker> TargetList { get; set; } = new();
        public bool IsReady => !TargetList.IsNullOrEmpty() && TargetList.Count > 0;
        
        private void Awake()
        {
            Flag = EntityType.Target;
        }

        // TODO. 좀 자신이 없다;
        // private void Filtering(IEnumerable<ICombatTaker> targetList)
        // {
        //     var suit = targetList.Where(x
        //         => x.TargetObject.GetComponent<ICombatTaker>() != null &&
        //            x.TargetObject.IsInLayerMask(targetLayer)) as List<ICombatTaker>;
        //
        //     if (suit.IsNullOrEmpty()) return;
        //
        //     var countFilter = targetCount >= suit.Count
        //         ? suit.Count
        //         : targetCount;
        //
        //     TargetList.Clear();
        //     TargetList.AddRange(suit.Take(countFilter));
        // }

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
