using Character.Villains;
using SceneAdaption;
using UnityEngine;

namespace Raid.UI.StageFinisher
{
    public class StageFinalizer : MonoBehaviour
    {
        public void Initialize(VillainBehaviour bossBehaviour)
        {
            // bossBehaviour.CharacterBehaviour.
        }
        
        public void ShowFinalizer()
        {
            gameObject.SetActive(true);
        }
        
        public void Retry()
        {
            SceneManager.LoadScene("Raid");
            // MainManager.Scene.LoadScene("Raid");
        }
    }
}
