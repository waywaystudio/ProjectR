using UnityEngine;

namespace Common.Projectors
{
    public class ArcProjector : ProjectorComponent
    {
        [SerializeField] private Transform colliderTransform;

        protected override void Awake()
        {
            base.Awake();
            
            Builder
                .Add(Section.Active, "ActiveCollider", ActiveCollider)
                .Add(Section.End, "DeActiveCollider", DeActiveCollider);
        }

        private void ActiveCollider() => colliderTransform.gameObject.SetActive(true);
        private void DeActiveCollider() => colliderTransform.gameObject.SetActive(false);
    }
}
