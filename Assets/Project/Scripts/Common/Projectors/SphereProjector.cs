using System;
using UnityEngine;

namespace Common.Projectors
{
    public class SphereProjector : ProjectorComponent
    {
        private IHasProjectorEntity subject;
        private Func<Vector2> sizeReference;

        private void Awake()
        {
            subject = GetComponent<IHasProjectorEntity>();
            
            CastingTime = subject.Progress.Value;
            
            subject.Progress.Register("Projector", value => value++);
            sizeReference += () => subject.SizeValue;
            
            if (!TryGetComponent(out ISequence MainSequence))
            {
                Debug.LogError($"Require ISequence. Call:{gameObject.name}");
                return;
            }
            
            projector.material = new Material(materialReference);
            
            // MainSequence.OnActivated.Register("Projector", Activate);
            // MainSequence.OnCanceled.Register("Projector", Cancel);
            // MainSequence.OnCompleted.Register("Projector", Complete);
            // MainSequence.OnEnded.Register("Projector", End);
        }
        
        public void MayBeActive()
        {
            var sizeVector = sizeReference.Invoke();
            
            projector.size = new Vector3(sizeVector.x * 2f, sizeVector.y * 2f, ProjectorDepth);
        }

        // CastingTime은 FloatEvent를 쓸 수 있을 것 같다.
        // Radius는 Func<float>으로 받을 수도 있다.
        // Width, Height를 받아야 하기도 한다.

        

        public void Initialize(float castingTime, float radius, ISequence mainSequence)
        {
            CastingTime           = castingTime;
            projector.size        = new Vector3(radius * 2f, radius * 2f, ProjectorDepth);
            
            CoreInitialize(mainSequence);
        }
    }
}
