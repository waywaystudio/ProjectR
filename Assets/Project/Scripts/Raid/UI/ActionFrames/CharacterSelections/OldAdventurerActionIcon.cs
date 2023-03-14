using Character.Adventurers;
using GameEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Raid.UI.ActionFrames.ActionBars.AdventurerBars
{
    public class OldAdventurerActionIcon : MonoBehaviour
    {
        [SerializeField] private Image adventurerImage;
        [SerializeField] private GameEvent<Adventurer> onFocusChanged;

        private Adventurer ab;

        public void Initialize(Adventurer adventurer)
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
