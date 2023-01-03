using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Character.Graphic.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private Image fillImage;
        [SerializeField] private float lerpDuration = 0.2f;

        private int instanceID;
        private readonly FunctionTable<float> maxHp = new();

        private void Awake()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            instanceID = GetInstanceID();
        }

        private void Start()
        {
            maxHp.Register(instanceID, () => cb.StatTable.MaxHp);
            cb.Status.OnHpChanged.Register(instanceID, FillHealth);

            FillHealth(cb.Status.Hp);
        }

        private void OnDisable() => cb.Status.OnHpChanged.Unregister(instanceID);

        private void FillHealth(float value)
        {
            var valueNormalize = value / maxHp.Invoke();
        
            fillImage.DOFillAmount(valueNormalize, lerpDuration);
        }
    }
}
