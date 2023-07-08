using UnityEngine;

namespace Common.Particles
{
    public class CombatParticle : MonoBehaviour
    {
        [SerializeField] private ParticlePool pool;
        [SerializeField] private string actionKey;
        [SerializeField] private ParticleSpawnParent parent;
        [SerializeField] private PrepositionType prepositionType;
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
                if (prepositionType == PrepositionType.None) return transform.position;
                
                var taker = takerHolder.Taker;
            
                return taker is null 
                    ? transform.position 
                    : taker.Preposition(prepositionType).position;
            }
        }


        public void Initialize(Sequencer sequencer, IHasTaker takerHolder)
        {
            this.takerHolder = takerHolder;
            
            var builder = new SequenceBuilder(sequencer);

            if (playSection != Section.None) builder.Add(playSection, actionKey, PlayParticle);
            if (endSection  != Section.None) builder.Add(endSection, actionKey, StopParticle);

            pool.Initialize(component => component.Pool = pool, Parent);
        }

        public void Dispose()
        {
            pool?.Clear();
        }


        private void PlayParticle()
        {
            pool.Get().Play(SpawnPosition, Parent);
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
