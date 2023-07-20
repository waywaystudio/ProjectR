using Sirenix.OdinInspector;
using UnityEngine;

namespace Common.Projectors
{
    public class DonutProjector : ProjectorComponent
    {
        [SerializeField] private bool enableCollider;
        [SerializeField] private GameObject colliderObject;
        [PropertyRange(0, 1)]
        [SerializeField] private float fillSimulator;
        [SerializeField] private float sizeSimulator;
        [PropertyRange(0, 1)]
        [SerializeField] private float innerRadiusSimulator;
        
        protected static readonly int InnerRadiusID = Shader.PropertyToID("_InnerRadius");
        
        public override void Initialize(IProjection provider)
        {
            base.Initialize(provider);
            
            projector.material.SetFloat(InnerRadiusID, innerRadiusSimulator);

            var builder = new CombatSequenceBuilder(provider.Sequence);
            
            if (enableCollider)
            {
                colliderObject.transform.localScale *= Provider.SizeEntity.AreaRange;
                
                builder
                    .Add(Section.Active, $"{InstanceKey}.ActiveCollider", () => colliderObject.SetActive(true))
                    .Add(Section.End,  $"{InstanceKey}.DeActiveCollider", () => colliderObject.SetActive(false))
                    ;
            }
            else
            {
                colliderObject.SetActive(false);
            }
        }
        
        
        /// <summary>
        /// Updates the head projector properties.
        /// </summary>
        private void UpdateHeadProjector()
        {
            if (materialReference == null || projector.IsNullOrDestroyed()) return;

            var currentSize = projector.size;
            
            currentSize.x = sizeSimulator * 2f;
            currentSize.y = sizeSimulator * 2f;
            
            projector.size = currentSize;
        
            projector.material.SetColor(ColorShaderID, backgroundColor);
            projector.material.SetColor(FillColorShaderID, fillColor);
            projector.material.SetFloat(FillProgressShaderID, fillSimulator);
            projector.material.SetFloat(InnerRadiusID, innerRadiusSimulator);
        }
        
        private void OnValidate()
        {
            UpdateHeadProjector();
        }
    }
}
