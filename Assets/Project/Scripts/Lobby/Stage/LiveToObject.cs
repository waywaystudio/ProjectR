using DG.Tweening;
using UnityEngine;

namespace Lobby.Stage
{
    public class LiveToObject : MonoBehaviour
    {
        private Tween bounceTween;

        private void OnEnable()
        {
            var destination = transform.position + Vector3.up * 1f;

            bounceTween = transform
                          .DOMove(destination, .5f)
                          .SetEase(Ease.OutQuad)
                          .SetLoops(-1, LoopType.Yoyo);
        }

        private void OnDisable()
        {
            bounceTween?.Kill();
        }
    }
}
