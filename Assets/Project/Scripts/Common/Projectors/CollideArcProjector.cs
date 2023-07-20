using UnityEngine;

namespace Common.Projectors
{
    public class CollideArcProjector : ArcProjector
    {
        [SerializeField] private Transform colliderTransform;

        public override void Initialize(IProjection provider)
        {
            base.Initialize(provider);

            colliderTransform.localScale *= Provider.SizeEntity.AreaRange;
            DeActiveCollider();

            var builder = new CombatSequenceBuilder(provider.Sequence);
            
            builder
                .Add(Section.Active, $"{InstanceKey}.ActiveCollider", ActiveCollider)
                .Add(Section.End, $"{InstanceKey}.DeActiveCollider", DeActiveCollider);
        }

        private void ActiveCollider() => colliderTransform.gameObject.SetActive(true);
        private void DeActiveCollider() => colliderTransform.gameObject.SetActive(false);
    }
}
