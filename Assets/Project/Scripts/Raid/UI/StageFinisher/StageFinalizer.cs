using Character;
using Character.Monsters;
using Common;
using Manager;
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
            MainManager.Scene.LoadScene("Raid");
            MainAdventurer.Return();
        }
    }
}
