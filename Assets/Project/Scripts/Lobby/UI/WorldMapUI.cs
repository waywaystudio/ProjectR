using Common;
using GameEvents;
using UnityEngine;

namespace Lobby.UI
{
    public class WorldMapUI : MonoBehaviour, IEditable
    {
        [SerializeField] private GameEvent onFocusVillainUIChanged;
        [SerializeField] private GameEvent onDifficultyChanged;

        public VillainType FocusVillain
        {
            get => Den.StageVillain;
            set
            {
                Den.StageVillain = value;
                onFocusVillainUIChanged.Invoke();
            }
        }

        public DifficultyType Difficulty
        {
            get => Den.Difficulty;
            set
            {
                Den.Difficulty = value;
                onDifficultyChanged.Invoke();
            }
        }

        public void NextDifficulty()
        {
            Difficulty = Difficulty.NextExceptNone();
        }

        public void PrevDifficulty()
        {
            Difficulty = Difficulty.PrevExceptNone();
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            
        }
#endif
    }
}
