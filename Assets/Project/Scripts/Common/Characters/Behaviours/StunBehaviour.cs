using Common.Effects.Particles;
using UnityEngine;

namespace Common.Characters.Behaviours
{
    public class StunBehaviour : MonoBehaviour, IActionBehaviour
    {
        [SerializeField] private bool isImmune;
        [SerializeField] private ParticlePool pool;
        
        public ActionMask BehaviourMask => ActionMask.Stun;
        public Sequencer<float> Sequencer { get; } = new();
        public SequenceBuilder<float> Builder { get; private set; }
        public SequenceInvoker<float> Invoker { get; private set; }

        private CharacterBehaviour cb;
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        private InstanceTimer Timer { get; } = new();
        

        public void Stun(float duration)
        {
            if (!Invoker.IsAbleToActive) return;
            
            Invoker.Active(duration);
        }

        public void Cancel()
        {
            Invoker.Cancel();
            Timer.Stop();
        }
        
        public void Immune(bool value) => isImmune = value;


        private void PlayParticle()
        {
            var spawnPosition = Cb.Preposition(PrepositionType.Top);
            
            pool.Get().Play(spawnPosition.position, transform);
        }

        private void StopParticle()
        {
            pool.Release();
        }


        private void OnEnable()
        {
            Invoker = new SequenceInvoker<float>(Sequencer);
            Builder = new SequenceBuilder<float>(Sequencer);
            Builder
                .AddCondition("AbleToBehaviourOverride", () => BehaviourMask.CanOverride(Cb.BehaviourMask))
                .AddCondition("IsNotImmune", () => !isImmune)
                .AddActiveParam("PlayStunTimer", duration => Timer.Play(duration, Invoker.Complete))
                .Add(Section.Active,"CancelPreviousBehaviour", () => cb.CurrentBehaviour?.TryToOverride(this))
                .Add(Section.Active, "PlayParticle", PlayParticle)
                .Add(Section.Active,"SetCurrentBehaviour", () => cb.CurrentBehaviour = this)
                .Add(Section.Active,"PlayAnimation", Cb.Animating.Stun)
                .Add(Section.End,"Stop", Cb.Stop)
                .Add(Section.End,"StopParticle", StopParticle)
                ;

            pool.Initialize(component => component.Pool = pool, transform);
        }

        private void OnDisable()
        {
            Sequencer.Clear();
            Timer.Stop();
        }
    }
}
