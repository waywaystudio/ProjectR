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
        private IHasTaker takerHolder;
        private Transform Parent
        {
            get
            {
                var taker = takerHolder?.Taker;

                return parent switch
                {
                    ParticleSpawnParent.Provider => transform.parent,
                    ParticleSpawnParent.Taker    => taker is null ? null : taker.StatusEffectHierarchy,
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
                
                var taker = takerHolder.Taker;
            
                return taker is null 
                    ? transform.position 
                    : taker.Preposition(takerPreposition).position;
            }
        }

        public bool Activity { get; set; } = true;


        public void Initialize(CombatSequence sequence, IHasTaker takerHolder)
        {
            this.takerHolder = takerHolder;
            
            var builder = new CombatSequenceBuilder(sequence);

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


        private void PlayParticle(Vector3 position, Transform parent)
        {
            if (!Activity) return;
            
            pool.Get().Play(position, parent);
        }

        private void PlayParticle()
        {
            if (!Activity) return;
            
            pool.Get().Play(SpawnPosition, Parent);
        }

        private void PlayParticle(ICombatTaker taker)
        {
            if (!Activity) return;
            
            pool.Get().Play(SpawnPosition, taker.StatusEffectHierarchy);
        }

        /// <summary>
        /// Only for SingleInstance type
        /// </summary>
        private void StopParticle()
        {
            pool.Release();
        }
    }
}