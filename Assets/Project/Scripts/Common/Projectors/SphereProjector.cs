using UnityEngine;

namespace Common.Projectors
{
    public class SphereProjector : ProjectorComponent
    {
        [SerializeField] private SphereCollider obstacleCollider;

        protected override void Awake()
        {
            base.Awake();

            obstacleCollider.radius = Provider.SizeVector.y;
            Builder
                .Add(SectionType.Active, "ActiveCollider", ActiveCollider)
                .Add(SectionType.End, "DeActiveCollider", DeActiveCollider);
        }

        private void ActiveCollider() => obstacleCollider.gameObject.SetActive(true);
        private void DeActiveCollider() => obstacleCollider.gameObject.SetActive(false);
    }
}
