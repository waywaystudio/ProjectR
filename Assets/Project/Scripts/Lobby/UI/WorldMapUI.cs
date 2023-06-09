using Common;
using Common.Characters;
using GameEvents;
using UnityEngine;

namespace Lobby.UI
{
    public class WorldMapUI : MonoBehaviour, IEditable
    {
        [SerializeField] private GameEvent onFocusVillainUIChanged;
        [SerializeField] private GameObject challengeGroup;

        public VillainType FocusVillain
        {
            get => Den.StageVillain;
            set
            {
                Den.StageVillain = value;
                onFocusVillainUIChanged.Invoke();
            }
        }

        public VillainData FocusVillainData => Den.GetVillainData(FocusVillain);

        public void OnFocusVillainUIChanged()
        {
            if (!challengeGroup.activeSelf)
            {
                challengeGroup.SetActive(true);
            }
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            challengeGroup = transform.Find("Contents").Find("ChallengeGroup").gameObject;
        }
#endif
    }
}
