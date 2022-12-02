using Core;
using UnityEngine;

namespace Common.Character.Skills.Entity
{
    public class RangeEntity : EntityAttribution, IRangeEntity, IReadyRequired
    {
        [SerializeField] private float range;
        
        public float Range { get => range; set => range = value; }
        public GameObject Target { get; set; }
        public bool IsReady
        { 
            get
            {
                if (Target.IsNullOrEmpty()) return false;
                return Vector3.Distance(Target.transform.position, transform.position) <= Range;
            }
        }

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
