using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Common.Particles
{
    public class CombatParticle : MonoBehaviour
    {
        [SerializeField] private ParticlePool pool;
        [SerializeField] private string actionKey;
        [SerializeField] private ParticleSpawnParent parent;
        [FormerlySerializedAs("prepositionType")] [SerializeField] private PrepositionType takerPreposition;
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


        public void Initialize(CombatSequence sequencer, IHasTaker takerHolder)
        {
            this.takerHolder = takerHolder;
            
            var builder = new CombatSequenceBuilder(sequencer);

            if (playSection != Section.None)
            {
                if (playSection == Section.Hit)
                {
                    builder.AddHit(actionKey, PlayParticle);
                }
                else
                {
                    builder.Add(playSection, actionKey, PlayParticle);
                }
            }
            
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

        private void PlayParticle(ICombatTaker taker)
        {
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
