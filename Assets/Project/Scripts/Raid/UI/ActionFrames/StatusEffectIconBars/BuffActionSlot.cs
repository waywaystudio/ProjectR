using Character.StatusEffect;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.ActionFrames.StatusEffectIconBars
{
    public class BuffActionSlot : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI remainDuration;

        public IStatusEffect StatusEffect { get; private set; }
        

        public void Register(IStatusEffect statusEffect)
        {
            Unregister();

            gameObject.SetActive(true);

            if (!Database.StatusEffectMaster.Get(statusEffect.ActionCode, out var seComponent))
            {
                Debug.LogError($"Cant not find StatusEffectComponent. Input:{statusEffect.ActionCode}");
                return;
            }

            StatusEffect        = statusEffect;
            icon.sprite         = seComponent.GetComponent<StatusEffectComponent>().Icon;
            remainDuration.text = statusEffect.Duration.ToString("F1");

            statusEffect.ProgressTime.Register("SetRemainDurationText", SetRemainDurationText);
            statusEffect.OnEnded.Register("RemoveUI", Unregister);
        }

        public void Unregister()
        {
            if (StatusEffect is null) return;

            icon.sprite         = null;
            remainDuration.text = string.Empty;
            StatusEffect.ProgressTime.Unregister("SetRemainDurationText");
            StatusEffect = null;
            
            gameObject.SetActive(false);
        }


        private void SetRemainDurationText(float value)
        {
            remainDuration.text = value.ToString("F1");
        }
    }
}
