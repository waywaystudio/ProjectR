using Common;
using Common.PartyCamps;
using TMPro;
using UnityEngine;

namespace Lobby.UI.Forge.Evolves
{
    public class SetRelicDescription : MonoBehaviour, IEditable
    {
        [SerializeField] private int setPiece;
        [SerializeField] private TextMeshProUGUI titleMesh;
        [SerializeField] private TextMeshProUGUI descriptionMesh;
        [SerializeField] private Color activeColor;
        [SerializeField] private Color deActiveColor;

        private RelicType CurrentRelicType { get; set; }


        public void ReloadRelicDescription()
        {
            if (LobbyDirector.UI.Forge.FocusRelic == CurrentRelicType) return;

            CurrentRelicType = LobbyDirector.UI.Forge.FocusRelic;
            
            var targetRelicType = LobbyDirector.UI.Forge.FocusRelic;
            var relic           = Database.RelicData(targetRelicType.ConvertToDatIndex());
            var descriptionText = setPiece switch
            {
                2 => relic.Set2Desc,
                4 => relic.Set4Desc,
                6 => relic.Set6Desc,
                _ => "None",
            };

            SetRelicText(descriptionText);
            SetTextColor(targetRelicType);
        }

        private void SetRelicText(string text)
        {
            titleMesh.text       = $"{CurrentRelicType.ToString()} {setPiece} Set Effect.";
            descriptionMesh.text = text;
        }
        
        private void SetTextColor(RelicType type)
        {
            var focusAdventurer     = LobbyDirector.UI.Forge.FocusAdventurer;
            var ad                  = PartyCamp.Characters.GetData(focusAdventurer);
            var characterEthosCount = ad.GetRelicPieceCount(type);

            descriptionMesh.color = titleMesh.color = characterEthosCount >= setPiece
                ? activeColor
                : deActiveColor;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            titleMesh            = transform.Find("SetTitle").GetComponent<TextMeshProUGUI>();
            descriptionMesh      = transform.Find("SetDescription").GetComponent<TextMeshProUGUI>();
            titleMesh.text       = $"Set {setPiece} Effect.";
            descriptionMesh.text = $"#{setPiece} Set Description";
        }
#endif
    }
}
