using System.Collections.Generic;
using Common.Characters.Behaviours;
using Destructible2D;
using DG.Tweening;
using UnityEngine;

namespace Character.Dummies.Behaviours
{
    public class DummyDeadBehaviour : DeadBehaviour
    {
        [SerializeField] private Vector2 jumpPowerRange = new (1.0f, 2.4f);
        [SerializeField] private Vector2 jumpDurationRange = new (0.15f, 0.4f);
        [SerializeField] private float maxSpreadRange = 3f;
        [SerializeField] private D2dDestructibleSprite destructible;
        [SerializeField] private D2dFracturer fracture;

        private static float RandomRangeOne => Random.Range(-1.0f, 1.0f);
        private static readonly Color FadeColor = new (0, 0, 0, 0);
        private InstanceTimer BanishTimer { get; } = new();
        private DummyBehaviour vb;
        private DummyBehaviour Vb => vb ??= GetComponentInParent<DummyBehaviour>();

        public void Fracture()
        {
            fracture.TryFracture();
        }
        
        
        private void Explode(List<D2dDestructible> fractures, D2dDestructible.SplitMode mode)
        {
            fractures.ForEach(fracture =>
            {
                var pivot = transform.position;
                var jumpPower = Random.Range(jumpPowerRange.x, jumpPowerRange.y);
                var jumpDuration = Random.Range(jumpDurationRange.x, jumpDurationRange.y);
                var destination = new Vector3(pivot.x + (RandomRangeOne * maxSpreadRange), 0f, pivot.z + (RandomRangeOne * maxSpreadRange));
        
                fracture.transform.DOJump(destination, jumpPower, 1, jumpDuration);
            });
        }

        private void Banish()
        {
            var db = GetComponentInParent<DummyBehaviour>();

            db.SpriteRenderer
              .DOColor(FadeColor, 1.0f)
              .OnComplete(OnDisable);
        }
        
        protected override void OnEnable()
        {
            destructible.OnSplitEnd += Explode;
            
            Invoker = new SequenceInvoker(Sequence);
            Builder = new SequenceBuilder(Sequence);
            Builder
                .Add(Section.Active, "SetCurrentBehaviour", () => Vb.CurrentBehaviour = this)
                .Add(Section.Active, "Fracture", Fracture)
                .Add(Section.End, "Banish", () => BanishTimer.Play(1.0f, Banish));
                ;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            destructible.OnSplitEnd -= Explode;
        }
    }
}
