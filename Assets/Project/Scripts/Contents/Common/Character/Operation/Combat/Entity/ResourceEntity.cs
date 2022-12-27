using UnityEngine;

namespace Common.Character.Operation.Combat.Entity
{
    public class ResourceEntity : BaseEntity
    {
        [SerializeField] private float obtain;

        // TODO. 현재 CoolTime 기준으로 받아오고 있어서 Resource.IsReady 메카닉 오류남.
        public override bool IsReady => obtain switch
        {
            < 0 => Sender.Status.Resource >= obtain * -1f,
            _ => true,
        };
        
        public float Obtain { get => obtain; set => obtain = value; }

        private void OnEnable() => OnCompleted.Register(InstanceID, () => Sender.Status.Resource += Obtain);
        private void OnDisable() => OnCompleted.Unregister(InstanceID);
    }
}
