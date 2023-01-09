using Core;
using UnityEngine;

namespace Character.Combat.Entities
{
    public class ResourceEntity : BaseEntity
    {
        [SerializeField] private float obtain;

        public override bool IsReady => obtain switch
        {
            < 0 => Provider.DynamicStatEntry.Resource.Value >= obtain * -1f,
            _ => true,
        };
        
        public float Obtain { get => obtain; set => obtain = value; }

        private void OnEnable() => OnCompleted.Register(InstanceID, () => Provider.DynamicStatEntry.Resource.Value += Obtain);
        private void OnDisable() => OnCompleted.Unregister(InstanceID);
        
        
#if UNITY_EDITOR
        public void SetUpValue(float obtain)
        {
            Obtain = obtain;
            Flag   = EntityType.Resource;
        }
#endif
    }
}
