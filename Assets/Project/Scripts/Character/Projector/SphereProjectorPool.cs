using Core;

namespace Character.Projector
{
    public class SphereProjectorPool : Pool<SphereProjector>
    {
        private float castingTime;
        private float radius;
        private ICombatTaker taker;
        
        public void Initialize(float castingTime, float radius, ICombatTaker taker)
        {
            this.castingTime = castingTime;
            this.radius      = radius;
            this.taker       = taker;
        }
        
        protected override void OnGetPool(SphereProjector element)
        {
            element.gameObject.SetActive(true);
            element.Initialize(castingTime, radius);
        }

        protected override void OnReleasePool(SphereProjector element)
        {
            element.gameObject.SetActive(false);
            element.transform.SetParent(Origin, false);
        }

        protected override void OnDestroyPool(SphereProjector element)
        {
            Destroy(element.gameObject);
        }
    }
}
