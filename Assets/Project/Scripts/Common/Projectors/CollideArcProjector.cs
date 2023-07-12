using UnityEngine;

namespace Common.Projectors
{
    public class CollideArcProjector : ArcProjector
    {
        [SerializeField] private Transform colliderTransform;

        public override void Initialize(IProjectionProvider provider)
        {
            base.Initialize(provider);

            var builder = new CombatSequenceBuilder(provider.Sequence);
            
            builder
                .Add(Section.Active, "ActiveCollider", ActiveCollider)
                .Add(Section.End, "DeActiveCollider", DeActiveCollider);
        }

        private void ActiveCollider() => colliderTransform.gameObject.SetActive(true);
        private void DeActiveCollider() => colliderTransform.gameObject.SetActive(false);
    }
}
