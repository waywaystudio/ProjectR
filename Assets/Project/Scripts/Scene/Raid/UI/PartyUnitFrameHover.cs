using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scene.Raid.UI
{
    public class PartyUnitFrameHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        // [SerializeField] private Transform hoverObject;
        [SerializeField] private float duration = 0.15f;
        [SerializeField] private float targetScale = 1.03f;
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScaleX(targetScale, duration);
            transform.DOScaleY(targetScale, duration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScaleX(1f, duration);
            transform.DOScaleY(1f, duration);
        }

#if UNITY_EDITOR
        private void SetUp()
        {
            // hoverObject ??= transform;
        }
#endif
    }
}
