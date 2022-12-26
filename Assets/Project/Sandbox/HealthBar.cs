using Common.Character;
using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private CharacterBehaviour cb;
    [SerializeField] private Image fillImage;
    [SerializeField] private float lerpDuration = 0.2f;

    private readonly FunctionTable<float> maxHp = new();

    private void Awake()
    {
        cb = GetComponentInParent<CharacterBehaviour>();
    }

    private void Start()
    {
        maxHp.Register(GetInstanceID(), () => cb.StatTable.MaxHp);
        cb.OnHpChanged.Register(GetInstanceID(), FillHealth);
    }

    [Button]
    private void SetPlayerHp(float value)
    {
        cb.Hp = value;
    }

    private void FillHealth(float value)
    {
        var valueNormalize = value / maxHp.Invoke();
        
        fillImage.DOFillAmount(valueNormalize, lerpDuration);
    }
}
