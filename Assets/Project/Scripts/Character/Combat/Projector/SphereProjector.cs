using Core;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character.Combat.Projector
{
    public class SphereProjector : ProjectorObject, IEditorSetUp
    {
        // # float modifier;
        // # DecalProjector decal;
        // # Material decalMaterial;

        private float castingTime;
        private static readonly int FillAmount = Shader.PropertyToID("_FillAmount");
        

        public override void Generate(Vector3 pointA, Vector3 pointB)
        {
            projectorCollider.Generate(pointA, pointB);

            Play();
        }
        

        [Button]
        protected override void Play()
        {
            decalMaterial
                .DOFloat(1.5f, FillAmount, castingTime)
                .OnComplete(OnCompleteGenerate);
        }

        protected void OnCompleteGenerate()
        {
            decalMaterial.SetFloat(FillAmount, 0f);
        }


#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();
        }
#endif
    }
}
