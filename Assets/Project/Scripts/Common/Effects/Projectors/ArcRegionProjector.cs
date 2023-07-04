using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

namespace Common.Effects.Projectors
{
    public class ArcRegionProjector : MonoBehaviour, IEditable
    {
        [SerializeField] private GameObject circle;
        [SerializeField] private Material circleMat;
        [SerializeField] private DecalProjector arcProjector;
        
        [SerializeField] private float radius = 1.0f;
        [SerializeField] private float angle;
        [SerializeField] private float arc = 30.0f;
        [SerializeField] private float fillProgress;
        [SerializeField] private float depth = 10;
        
        [SerializeField] private Color circleBaseColor = Color.white;
        [SerializeField] private Color circleFillColor = Color.white;

        private static readonly int ColorShaderID = Shader.PropertyToID("_Color");
        private static readonly int FillColorShaderID = Shader.PropertyToID("_FillColor");
        private static readonly int FillProgressShaderID = Shader.PropertyToID("_FillProgress");
        private static readonly int ArcShaderID = Shader.PropertyToID("_Arc");
        private static readonly int AngleShaderID = Shader.PropertyToID("_Angle");

        public float FillProgress { get => fillProgress; set => fillProgress = Mathf.Clamp01(value); }
        public float Radius { get => radius; set => radius = Mathf.Max(value, 0f); }
        public float Arc { get => arc; set => arc = Mathf.Clamp(value, 0f, 360f); }
        public float Angle { get => angle; set => angle = value; }
        
        public float ArcAngleNormalized => 1f - Arc / 360;
        public float NormalizedAngle => Mathf.Repeat((Angle - 90) % 360, 360) / 360;

        public void Initialize(float progress, float radius, float arc, float angle)
        {
            if (arcProjector != null)
            {
                arcProjector.material = new Material(arcProjector.material);
            }

            Fill();
        }

        public void Active(float duration)
        {
            
        }

        public void DeActive()
        {
            
        }
        
        
        private void Fill()
        {
            arcProjector.material.SetFloat(FillProgressShaderID, fillProgress);
        }


        [Sirenix.OdinInspector.Button]
        public void GenerateProjector()
        {
            arcProjector.material = circleMat;
            
            if (arcProjector != null)
                arcProjector.material = new Material(arcProjector.material);

            UpdateProjector();
        }


        private void UpdateProjector()
        {
            if (arcProjector == null)
                return;

            var currentSize = arcProjector.size;
            
            currentSize.x         = radius;
            currentSize.y         = radius;
            currentSize.z         = depth;
            arcProjector.pivot = new Vector3(0, 0, depth /2);
            arcProjector.size  = currentSize;

            if (arcProjector.material == null)
                return;

            arcProjector.material.SetColor(ColorShaderID, circleBaseColor);
            arcProjector.material.SetColor(FillColorShaderID, circleFillColor);
            arcProjector.material.SetFloat(FillProgressShaderID, fillProgress);
            arcProjector.material.SetFloat(ArcShaderID, ArcAngleNormalized);
            arcProjector.material.SetFloat(AngleShaderID, NormalizedAngle);
        }

        /// <summary>
        /// Update projectors if there are any changes made in the inspector.
        /// </summary>
        private void OnValidate() => UpdateProjector();


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            arcProjector = circle.GetComponent<DecalProjector>();
            
            if(arcProjector == null)
                arcProjector = circle.AddComponent<DecalProjector>();
            
            arcProjector.material = circleMat;
            arcProjector.material = new Material(arcProjector.material);
            Fill();
            OnValidate();
        }
#endif
    }
}
