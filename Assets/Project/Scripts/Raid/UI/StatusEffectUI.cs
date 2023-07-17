using System;
using Common.StatusEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI
{
    public class StatusEffectUI : MonoBehaviour, IEditable
    {
        [SerializeField] private Image effectIcon;
        [SerializeField] private TextMeshProUGUI timer;


        public bool IsRegistered => gameObject.activeSelf;
        public StatusEffect StatusEffect { get; private set; }
        
        public void Register(StatusEffect effect)
        {
            gameObject.SetActive(true);

            StatusEffect = effect;
            effectIcon.sprite = StatusEffect.Icon;

            StatusEffect.ProgressTime.RemoveListener("UpdateTimer");
            StatusEffect.ProgressTime.AddListener("UpdateTimer", SetTimer);

            SetTimer(StatusEffect.Duration);
        }

        public void Unregister()
        {
            gameObject.SetActive(false);
            
            effectIcon.sprite = null;
            timer.text        = string.Empty;
            StatusEffect      = null;
        }


        private void SetTimer(float value)
        {
            timer.text = value switch
            {
                <= 0.1f => "",
                <= 5f   => value.ToString("F1"),
                _       => value.ToString("F0"),
            };

        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            effectIcon = transform.Find("Icon").GetComponent<Image>();
            timer      = transform.Find("Timer").GetComponent<TextMeshProUGUI>();
        }
#endif
    }
}
