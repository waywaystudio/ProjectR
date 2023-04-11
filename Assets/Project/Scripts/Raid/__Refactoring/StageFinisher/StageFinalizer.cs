using Character.Bosses;
using SceneAdaption;
using UnityEngine;

namespace Raid.UI.StageFinisher
{
    public class StageFinalizer : MonoBehaviour
    {
        public void Initialize(Boss bossBehaviour)
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
