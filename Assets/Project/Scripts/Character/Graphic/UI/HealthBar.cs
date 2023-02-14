using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Graphic.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image fillImage;
        [SerializeField] private float lerpDuration = 0.2f;

        private ICombatTaker taker;

        private void Awake()
        {
            taker = GetComponentInParent<ICombatTaker>();
        }

        private void Start()
        {
            taker.DynamicStatEntry.Hp.Register("FillHealth", FillHealth);

            FillHealth(taker.DynamicStatEntry.Hp.Value);
        }

        private void OnDisable()
        {
            taker.DynamicStatEntry.Hp.Unregister("FillHealth");
        }

        private void FillHealth(float value)
        {
            var valueNormalize = value / taker.StatTable.MaxHp;
        
            fillImage.DOFillAmount(valueNormalize, lerpDuration);
        }
    }
}
