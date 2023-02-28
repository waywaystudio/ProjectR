using Character;
using Core.GameEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Raid.UI.ActionFrames.ActionBars.AdventurerBars
{
    public class AdventurerActionIcon : MonoBehaviour
    {
        [SerializeField] private Image adventurerImage;
        [SerializeField] private GameEvent<AdventurerBehaviour> onFocusChanged;

        private AdventurerBehaviour ab;

        public void Initialize(AdventurerBehaviour adventurer)
        {
            ab = adventurer;
            // SetAdventurerImage;
        }

        public void StartAction(InputAction.CallbackContext context)
        {
            onFocusChanged.Invoke(ab);
        }
    }
}
