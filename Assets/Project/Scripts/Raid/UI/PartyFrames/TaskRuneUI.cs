using Common.Runes;
using Common.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.PartyFrames
{
    public class TaskRuneUI : MonoBehaviour, IEditable
    {
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private Toggle toggle;
        [SerializeField] private ImageFiller imageFiller;
        [SerializeField] private Color completeColor;

        private EthosRune AssignedRune { get; set; }

        public void Initialize(EthosRune rune)
        {
            AssignedRune     = rune;
            description.text = rune.TaskDescription;
            
            AssignedRune.TaskProgress.AddListener("DeActiveProgressBar", OnComplete);
            imageFiller.RegisterEvent(rune.TaskProgress, rune.TaskGoal);
        }


        private void OnComplete()
        {
            if (AssignedRune.TaskProgress.Value < AssignedRune.TaskGoal) return;

            toggle.isOn = true;
            imageFiller.Image
                       .DOColor(completeColor, 0.1f)
                       .SetDelay(0.15f)
                       ;
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            description = transform.Find("TaskDescription").GetComponent<TextMeshProUGUI>();
            toggle      = GetComponentInChildren<Toggle>();
            imageFiller = GetComponentInChildren<ImageFiller>();
        }
#endif
    }
}
