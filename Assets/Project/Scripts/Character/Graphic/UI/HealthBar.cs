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

        private int instanceID;
        private ICombatTaker taker;

        private void Awake()
        {
            taker = GetComponentInParent<ICombatTaker>();
            instanceID = GetInstanceID();
        }

        private void Start()
        {
            taker.DynamicStatEntry.Hp.Register(instanceID, FillHealth);

            FillHealth(taker.DynamicStatEntry.Hp.Value);
        }

        private void OnDisable()
        {
            taker.DynamicStatEntry.Hp.Unregister(instanceID);
        }

        private void FillHealth(float value)
        {
            var valueNormalize = value / taker.StatTable.MaxHp;
        
            fillImage.DOFillAmount(valueNormalize, lerpDuration);
        }
    }
}
