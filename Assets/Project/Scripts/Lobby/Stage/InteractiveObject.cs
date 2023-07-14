using Character.Venturers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Lobby.Stage
{
    public class InteractiveObject : MonoBehaviour
    {
        [SerializeField] private string interactKey;
        [SerializeField] private SpriteRenderer balloon;
        [SerializeField] private UnityEvent<InputAction.CallbackContext> interactEvent;


        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out VenturerBehaviour player) && player == LobbyDirector.FocusVenturer)
            {
                // Add Event to Interaction;
                LobbyDirector.Input.InteractAction = interactEvent;
                balloon.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out VenturerBehaviour player) && player == LobbyDirector.FocusVenturer)
            {
                // Remove Event to Interaction;
                LobbyDirector.Input.InteractAction = null;
                LobbyDirector.DeActivePanels();
                balloon.gameObject.SetActive(false);
            }
        }
    }
}
