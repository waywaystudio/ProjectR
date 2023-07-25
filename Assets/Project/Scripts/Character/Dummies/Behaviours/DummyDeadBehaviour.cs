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
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private D2dDestructibleSprite destructible;
        [SerializeField] private D2dFracturer fracture;

        private readonly List<Tween> jumpTweenList = new ();
        private readonly List<Tween> fadeTweenList = new ();
        private int completedTween;
        private DummyBehaviour db;
        
        private static float RandomRangeOne => Random.Range(-1.0f, 1.0f);
        private static readonly Color FadeColor = new (0, 0, 0, 0);
        private DummyBehaviour Db => db ??= GetComponentInParent<DummyBehaviour>();
        

        public void Fracture()
        {
            fracture.TryFracture();
        }
        
        
        private void Explode(List<D2dDestructible> fractures, D2dDestructible.SplitMode mode)
        {
            jumpTweenList.Clear();
            completedTween = 0;
            
            fractures.ForEach(piece =>
            {
                var pivot = transform.position;
                var jumpPower = Random.Range(jumpPowerRange.x, jumpPowerRange.y);
                var jumpDuration = Random.Range(jumpDurationRange.x, jumpDurationRange.y);
                var destination = new Vector3(pivot.x + (RandomRangeOne * maxSpreadRange), 0f, pivot.z + (RandomRangeOne * maxSpreadRange));
            
                var tween = piece.transform
                                 .DOJump(destination, jumpPower, 1, jumpDuration)
                                 .OnComplete(() => Banish(piece));

                jumpTweenList.Add(tween);
            });
        }

        private void Banish(Component fracture)
        {
            if (fracture is not D2dDestructibleSprite fractureSprite) return;
            
            var fadeTween = fractureSprite.CachedSpriteRenderer
                                          .DOColor(FadeColor, fadeDuration)
                                          .OnComplete(DeActiveObject);

            fadeTweenList.AddUniquely(fadeTween);
        }

        private void DeActiveObject()
        {
            completedTween++;
                                              
            if (completedTween == jumpTweenList.Count)
            {
                Db.gameObject.SetActive(false);
            }
        }
        
        private void StopAllTween()
        {
            jumpTweenList.ForReverse(tween => tween.Kill());
            fadeTweenList.ForReverse(tween => tween.Kill());
            
            jumpTweenList.Clear();
            fadeTweenList.Clear();
        }
        
        protected override void OnEnable()
        {
            destructible.OnSplitEnd += Explode;
            
            Invoker = new SequenceInvoker(Sequence);
            Builder = new SequenceBuilder(Sequence);
            Builder
                .Add(Section.Active, "SetCurrentBehaviour", () => Db.CurrentBehaviour = this)
                .Add(Section.Active, "Fracture", Fracture)
                ;
        }
        
        protected override void OnDisable()
        {
            base.OnDisable();
            
            destructible.OnSplitEnd -= Explode;
            StopAllTween();
        }
    }
}