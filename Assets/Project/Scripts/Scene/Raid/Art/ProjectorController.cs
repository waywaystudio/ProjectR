using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Raid.Art
{
    public class ProjectorController : MonoBehaviour
    {
        [SerializeField] private DecalProjector projector;

        private Material decalMaterial;
        private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");

        public void FillOnce(float duration, Action callback = null)
        {
            Fill(duration,
                () =>
                {
                    callback?.Invoke();
                    Hide();
                });
        }
        
        public void Fill(float duration, Action callback = null)
        {
            Show();

            decalMaterial.DOFloat(1.0f, FillAmount, duration)
                .SetEase(Ease.InQuad)
                .OnComplete(() => callback?.Invoke());
        }

        public void Show()
        {
            gameObject.SetActive(true);
            
            ResetProjector();
        }
        
        public void ResetProjector()
        {
            decalMaterial.SetFloat(FillAmount, 0f);
        }

        public void Hide()
        {
            ResetProjector();
            
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            projector     ??= GetComponent<DecalProjector>();
            decalMaterial =   projector.material;
        }


        public void SetUp()
        {
            TryGetComponent(out projector);
        }
    }
}
