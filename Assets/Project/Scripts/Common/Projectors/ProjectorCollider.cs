using UnityEngine;

namespace Common.Projectors
{
    public class ProjectorCollider : MonoBehaviour, IAssociate<Projection>
    {
        public void Initialize(Projection master)
        {
            if (!TryGetComponent(out Collider _))
            {
                Debug.LogWarning($"Not Exist Collider in {gameObject.name}");
                return;
            }

            DeActiveCollider();
            
            var size = master.Provider.SizeEntity;
            var targetType = master.TargetingType;
            
            transform.localScale = targetType switch
            {
                TargetingType.Circle => new Vector3(size.AreaRange, size.AreaRange, size.AreaRange),
                TargetingType.Arc    => new Vector3(size.AreaRange, size.AreaRange, size.AreaRange),
                TargetingType.Rect   => new Vector3(size.Width, 1f, size.Height),
                TargetingType.Donut  => new Vector3(size.OuterRadius, size.OuterRadius, size.OuterRadius),
                _                    => Vector3.one
            };
            
            master.Builder
                .Add(Section.Active, $"{master.InstanceKey}.ActiveCollider", ActiveCollider)
                .Add(Section.End, $"{master.InstanceKey}.DeActiveCollider", DeActiveCollider);
        }
        

        private void ActiveCollider() => gameObject.SetActive(true);
        private void DeActiveCollider() => gameObject.SetActive(false);
    }
}
