using Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Character.Graphic.UI
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
            cb.DynamicStatEntry.Resource.Register(instanceID, FillResource);

            FillResource(cb.DynamicStatEntry.Resource.Value);
        }

        private void OnDisable() => cb.DynamicStatEntry.Resource.Unregister(instanceID);

        private void FillResource(float value)
        {
            var valueNormalize = value / maxResource.Invoke();
        
            fillImage.DOFillAmount(valueNormalize, lerpDuration);
        }
    }
}
