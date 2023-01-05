using UnityEngine;

namespace Character.Combat.Entities
{
    public class ResourceEntity : BaseEntity
    {
        [SerializeField] private float obtain;

        public override bool IsReady => obtain switch
        {
            < 0 => Provider.Status.Resource >= obtain * -1f,
            _ => true,
        };
        
        public float Obtain { get => obtain; set => obtain = value; }

        private void OnEnable() => OnCompleted.Register(InstanceID, () => Provider.Status.Resource += Obtain);
        private void OnDisable() => OnCompleted.Unregister(InstanceID);
    }
}
