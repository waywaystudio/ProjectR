using Common;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Tooltips
{
    [RequireComponent(typeof(ITooltipInfo))]
    public class TooltipDrawer : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Vector3 spawnOffset;
        
        private ITooltipInfo tooltipIncludedObject;
        private RectTransform rectTransform;
        private Tooltip currentTooltipObject;
        

        public void OnPointerEnter(PointerEventData eventData)
        {
            var spawnPosition = transform.position + spawnOffset;

            currentTooltipObject = 
                MainUI.TooltipPool.ShowToolTip(spawnPosition, tooltipIncludedObject.TooltipInfo);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MainUI.TooltipPool.Release(currentTooltipObject);
        }
        
        
        private void Awake()
        {
            TryGetComponent(out tooltipIncludedObject);
            TryGetComponent(out rectTransform);

            if (tooltipIncludedObject == null)
            {
                Debug.LogWarning("TooltipDrawer require ITooltipInfo interface included Component");
            }
        }
    }
}
