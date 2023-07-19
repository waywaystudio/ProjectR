using UnityEngine;

namespace Common.Effects.Particles
{
    public class CombatParticle : MonoBehaviour
    {
        [SerializeField] private ParticlePool pool;
        [SerializeField] private string actionKey;
        [SerializeField] private ParticleSpawnParent parent;
        [SerializeField] private PrepositionType takerPreposition;
        [SerializeField] private Section playSection;
        [SerializeField] private Section endSection;
        
        private ISequencerHolder sequenceHolder;
        private ICombatTaker Taker => takerRetriever?.Invoke();
        private System.Func<ICombatTaker> takerRetriever;
        private bool onEffecting;
        private Transform Parent
        {
            get
            {
                return parent switch
                {
                    ParticleSpawnParent.Provider => transform.parent,
                    ParticleSpawnParent.Taker    => Taker is null ? null : Taker.CombatStatusHierarchy,
                    ParticleSpawnParent.World    => null,
                    _                            => null
                };
            }
        }
        private Vector3 SpawnPosition
        {
            get
            {
                if (takerPreposition == PrepositionType.None) return transform.position;

                return Taker is null 
                    ? transform.position 
                    : Taker.Preposition(takerPreposition).position;
            }
        }

        public bool Activity { get; set; } = true;


        public void Initialize(ICombatObject combatObject)
        {
            takerRetriever += () => combatObject.Taker;
            
            var builder = new CombatSequenceBuilder(combatObject.Sequence);

            if (playSection != Section.None)
            {
                switch (playSection)
                {
                    case Section.Hit:
                    {
                        builder.AddHit(actionKey, PlayParticle);
                        break;
                    }
                    case Section.SubHit:
                    {
                        builder.AddSubHit(actionKey, PlayParticle);
                        break;
                    }
                    case Section.Fire:
                    {
                        builder.AddFire(actionKey, position => PlayParticle(position, Parent));
                        break;
                    }
                    case Section.SubFire:
                    {
                        builder.AddSubFire(actionKey, position => PlayParticle(position, Parent));
                        break;
                    }
                    default:
                    {
                        builder.Add(playSection, actionKey, PlayParticle);
                        break;
                    }
                }
            }
            
            if (endSection  != Section.None) builder.Add(endSection, actionKey, StopParticle);

            pool.Initialize(component => component.Pool = pool, transform.parent);
        }


        private void PlayParticle() => PlayParticle(SpawnPosition, Parent);
        private void PlayParticle(ICombatTaker taker) => PlayParticle(SpawnPosition, taker.CombatStatusHierarchy);
        private void PlayParticle(Vector3 position, Transform parent)
        {
            if (!Activity) return;
            
            pool.Get().Play(position, parent);
            onEffecting = true;
        }

        /// <summary>
        /// Only for SingleInstance type
        /// </summary>
        private void StopParticle()
        {
            if (!onEffecting) return;
            
            pool.Release();
            onEffecting = false;
        }
    }
}
