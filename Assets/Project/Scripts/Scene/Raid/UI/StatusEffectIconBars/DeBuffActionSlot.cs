using Character.StatusEffect;
using Core;
using MainGame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Raid.UI.StatusEffectIconBars
{
    public class DeBuffActionSlot : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private string fillActionKey;
        [SerializeField] private TextMeshProUGUI remainDuration;

        public StatusEffectEntity StatusEffectEntity { get; private set; }
        

        public void Register(StatusEffectEntity seEntity)
        {
            Unregister();

            gameObject.SetActive(true);

            MainData.StatusEffectMaster.Get(seEntity.Effect.ActionCode, out var seComponent);

            StatusEffectEntity  = seEntity;
            icon.sprite         = seComponent.GetComponent<StatusEffectComponent>().Icon;
            remainDuration.text = seEntity.Effect.Duration.ToString("F1");

            seEntity.Effect.ProcessTime.Register(fillActionKey, SetRemainDurationText);
            seEntity.Effect.OnEnded.Register("RemoveUI", Unregister);
        }

        public void Unregister()
        {
            if (StatusEffectEntity is null) return;

            icon.sprite         = null;
            remainDuration.text = string.Empty;
            StatusEffectEntity.Effect.ProcessTime.Unregister(fillActionKey);
            StatusEffectEntity = null;
            
            gameObject.SetActive(false);
        }


        private void SetRemainDurationText(float value)
        {
            remainDuration.text = value.ToString("F1");
        }
    }
}
