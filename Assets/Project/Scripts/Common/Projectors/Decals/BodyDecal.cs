using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Projectors.Projections
{
    /*
     * For Line Projector's Body */
    public class BodyDecal : Decal
    {
        /// <summary>
        /// Head & Body Line Projector는 독립적인 width와 length를 가짐
        /// </summary>
        [SerializeField] protected float width;
        [SerializeField] protected float length;
        
        
        /// <summary>
        /// The line head projector's Z offset, this lines up the head and body of the line. 
        /// </summary>
        protected const float ArrowZDisplacement = 2.9835f;
        

        /// <summary>
        /// The projector's 'y' position.
        /// </summary>
        protected const float YPosition = 0.15f;
        

        public override void Initialize(Projection master)
        {
            base.Initialize(master);

            UpdateBodyProjector();
        }
        
        
        protected override async UniTaskVoid PlayProjection()
        {
            gameObject.SetActive(true);
            DecalProjector.material.SetFloat(FillProgressShaderID, 0f);
            
            Cts = new CancellationTokenSource();
            
            var timer = 0f;
            var inverseDuration = 1.0f / Provider.CastingTime;

            while (timer < Provider.CastingTime)
            {
                timer += Time.deltaTime * inverseDuration;
                timer =  Mathf.Clamp01(timer);
                
                DecalProjector.material.SetFloat(FillProgressShaderID, timer * 2f);

                await UniTask.Yield(Cts.Token);
            }
        }
        
        
        /// <summary>
        /// Updates the body projector properties.
        /// </summary>
        private void UpdateBodyProjector()
        {
            var currentSize = DecalProjector.size;
            
            currentSize.x = length + 1 + (width - 1);
            currentSize.y = length + 1;
            currentSize.z = ProjectorDepth;

            DecalProjector.size  = currentSize;
            DecalProjector.transform.localPosition = 
                new Vector3(0, YPosition, (length + 1) * 0.5f);
        }
    }
}
