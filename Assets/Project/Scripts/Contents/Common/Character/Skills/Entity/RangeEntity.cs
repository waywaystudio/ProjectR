using Core;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class RangeEntity : EntityAttribution, IRangeEntity, IReadyRequired
    {
        [SerializeField] private float range;
        
        public float Range { get => range; set => range = value; }
        public Vector3 TargetPosition { get; set; } = Vector3.negativeInfinity;
        public bool IsReady => Vector3.Distance(TargetPosition, transform.position) <= Range;

#if UNITY_EDITOR
        protected override void OnEditorInitialize()
        {
            base.OnEditorInitialize();
            
            Flag = EntityType.Range;
            Range = StaticData.Range;
        }
#endif
    }
}
