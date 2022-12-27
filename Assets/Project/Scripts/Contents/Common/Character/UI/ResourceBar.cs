using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Character.UI
{
    public class ResourceBar : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        [SerializeField] private Image fillImage;
        [SerializeField] private float lerpDuration = 0.1f;

        private int instanceID;
        private readonly FunctionTable<float> maxResource = new();
        
        private void Awake()
        {
            cb ??= GetComponentInParent<CharacterBehaviour>();
            instanceID = GetInstanceID();
        }

        private void Start()
        {
            maxResource.Register(instanceID, () => cb.StatTable.MaxResource);
            cb.Status.OnResourceChanged.Register(instanceID, FillResource);

            FillResource(cb.Status.Hp);
        }

        private void OnDisable() => cb.Status.OnResourceChanged.Unregister(instanceID);

        private void FillResource(float value)
        {
            var valueNormalize = value / maxResource.Invoke();
        
            fillImage.DOFillAmount(valueNormalize, lerpDuration);
        }
    }
}
