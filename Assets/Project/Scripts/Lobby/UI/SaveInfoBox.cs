using SceneAdaption;
using Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby.UI
{
    public class SaveInfoBox : MonoBehaviour
    {
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
            saveButton.onClick.AddListener(info.Save);
            // saveButton.onClick.AddListener(Reload);
            
            loadButton.onClick.RemoveAllListeners();
            loadButton.onClick.AddListener(info.Load);
            loadButton.onClick.AddListener(SceneManager.ToLobbyScene);
            // loadButton.onClick.AddListener(Reload);
            
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(info.Delete);
            deleteButton.onClick.AddListener(Reload);
            deleteButton.onClick.AddListener(() => Destroy(gameObject));
        }
        
        private static void Reload() => LobbyDirector.SaveLoadFrame.Reload();
    }
}
