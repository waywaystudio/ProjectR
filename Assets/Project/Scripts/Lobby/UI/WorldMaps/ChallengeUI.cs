using Common;
using SceneAdaption;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.WorldMaps
{
    public class ChallengeUI : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        private static VillainType FocusVillain => LobbyDirector.WorldMap.FocusVillain;

        private void OnEnable()
        {
            SetEnable(false);
        }

        public void OnFocusVillainUIChanged()
        {
            if (FocusVillain != VillainType.None)
            {
                SetEnable(true);
            }
        }
        
        public void OnButtonClicked()
        {
            SceneManager.ToRaidScene();
        }
        
        
        private void SetEnable(bool value)
        {
            var colorBlock = button.colors;
            
            if (value)
            {
                colorBlock.normalColor = Color.white;
                button.colors          = colorBlock;
                button.interactable    = true;
            }
            else
            {
                colorBlock.normalColor = Color.black;
                button.colors          = colorBlock;
                button.interactable    = false;
            }
        }
    }
}
