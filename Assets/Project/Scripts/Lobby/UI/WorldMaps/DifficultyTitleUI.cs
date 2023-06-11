using TMPro;
using UnityEngine;

namespace Lobby.UI.WorldMaps
{
    public class DifficultyTitleUI : MonoBehaviour, IEditable
    {
        [SerializeField] private TextMeshProUGUI difficultyTextMesh;

        public void OnDifficultyChanged()
        {
            difficultyTextMesh.text = Den.Difficulty.ToString();
        }

        private void OnEnable() => OnDifficultyChanged();


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            difficultyTextMesh = GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
