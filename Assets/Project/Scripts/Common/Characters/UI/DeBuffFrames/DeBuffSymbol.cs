using Common.StatusEffects;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Characters.UI.DeBuffFrames
{
    public class DeBuffSymbol : MonoBehaviour, IEditable
    {
        [SerializeField] private Image deBuffIcon;
        [SerializeField] private TextMeshProUGUI timer;
        
        [ShowInInspector]
        private StatusEffect effect;


        public void Register(StatusEffect effect)
        {
            this.effect = effect;
            this.effect.ProgressTime.RemoveListener("UpdateDeBuffSymbolTimer");

            deBuffIcon.sprite = this.effect.Icon;
            timer.text        = this.effect.Duration.ToString("F1");
            
            this.effect.ProgressTime.AddListener("UpdateDeBuffSymbolTimer", SetTimer);
        }

        public void Unregister()
        {
            deBuffIcon.sprite = null;
            timer.text        = string.Empty;
            effect            = null;
        }


        private void SetTimer(float value)
        {
            timer.text = value.ToString("F1");
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            deBuffIcon = transform.Find("Icon").GetComponent<Image>();
            timer      = transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
