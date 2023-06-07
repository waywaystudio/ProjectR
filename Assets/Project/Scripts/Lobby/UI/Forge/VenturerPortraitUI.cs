using Common;
using UnityEngine;

namespace Lobby.UI.Forge
{
    public class VenturerPortraitUI : MonoBehaviour
    {
        [SerializeField] private VenturerType venturer;

        public void OnSelected()
        {
            LobbyDirector.UI.Forge.FocusVenturer = venturer;
        }
    }
}
