using UnityEngine;

namespace Character.Combat.Projector.Colliders
{
    public class RectangleProjectorCollider
    {
        // [SerializeField] private BoxCollider boxCollider;
        // x = width
        // z = length
        // y = always 0f; -> ground;

        // protected override void Generate(Vector3 providerPosition, Vector3 targetPosition)
        // {
        //     projectorObject.transform.position = (providerPosition + targetPosition) * 0.5f;
        //     projectorObject.transform.LookAt(targetPosition);
        //     
        //     boxCollider.enabled = true;
        //     boxCollider.size    = GenerateBoxSize(providerPosition, targetPosition);
        // }
        //
        // protected override void OnFinish()
        // {
        //     boxCollider.enabled = false;
        // }
        
        // private Vector3 GenerateBoxSize(Vector3 providerPosition, Vector3 targetPosition) 
        //     => new (sizeValue, 2f, Vector3.Distance(providerPosition, targetPosition));
        //
    }
}
