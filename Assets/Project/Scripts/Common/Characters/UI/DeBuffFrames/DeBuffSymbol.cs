using Common.StatusEffect;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Characters.UI.DeBuffFrames
{
    public class DeBuffSymbol : MonoBehaviour, IEditable
    {
        [SerializeField] private Image deBuffIcon;
        [SerializeField] private TextMeshProUGUI timer;
        
        private IStatusEffect effect;
        
        // private DataIndex actionCode;
        // private CharacterBehaviour cb;
        // public CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();

        public void Register(IStatusEffect effect)
        {
            this.effect = effect;

            if (!Database.StatusEffectMaster.Get<StatusEffectComponent>(this.effect.ActionCode, out var effectPrefab))
            {
                Debug.LogError($"Cant not find StatusEffectComponent. Input:{this.effect.ActionCode}");
                return;
            }
            
            this.effect.ProgressTime.Unregister("UpdateDeBuffSymbolTimer");
            this.effect.OnEnded.Unregister("HideUI");

            deBuffIcon.sprite = effectPrefab.Icon;
            timer.text        = this.effect.Duration.ToString("F1");
            
            this.effect.ProgressTime.Register("UpdateDeBuffSymbolTimer", SetTimer);
            this.effect.OnEnded.Register("HideUI", Unregister);
        }

        public void Unregister()
        {
            deBuffIcon.sprite = null;
            timer.text        = string.Empty;
            effect = null;
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
