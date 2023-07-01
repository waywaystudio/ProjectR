using SceneAdaption;
using Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI.SaveLoad
{
    public class SaveInfoBox : MonoBehaviour
    {
        [SerializeField] private SceneManager sceneManager;
        [SerializeField] private SaveManager saveManager;
        [SerializeField] private TextMeshProUGUI saveName;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button deleteButton;
        

        public void Initialize(SaveInfo info)
        {
            saveName.text = info.Filename;
            timeText.text = info.SaveTime;
            
            saveButton.onClick.RemoveAllListeners();
            saveButton.onClick.AddListener(() => saveManager.SaveToFile(info.Filename));
            
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(() => saveManager.LoadFromFile(info.Filename));
            loadButton.onClick.AddListener(sceneManager.ToLobbyScene);
            
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => saveManager.DeleteSaveFile(info.Filename));
            deleteButton.onClick.AddListener(Reload);
            deleteButton.onClick.AddListener(() => Destroy(gameObject));
        }
        
        private static void Reload() => LobbyDirector.SaveLoadUI.Reload();
    }
}
