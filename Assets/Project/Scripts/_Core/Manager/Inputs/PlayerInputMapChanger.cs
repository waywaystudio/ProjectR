using UnityEngine;
using UnityEngine.EventSystems;

namespace Inputs
{
    public class PlayerInputMapChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnPointerEnter(PointerEventData eventData)
        {
            InputManager.OffRaidMap();
            Debug.Log("OffRaidMap");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            InputManager.OnRaidMap();
            Debug.Log("OnRaidMap");
        }
    }
}
