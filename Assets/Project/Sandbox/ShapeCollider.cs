using Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.U2D;

public class ShapeCollider : MonoBehaviour
{
    public LayerMask AdventurerLayer;
    public SpriteShapeController SpriteShapeController;
    public SpriteShapeRenderer SpriteShapeRenderer;

    [Button]
    private void TestButton()
    {
        //SpriteShapeController.
        // SpriteShapeRenderer.mat
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.IsInLayerMask(AdventurerLayer))
        {
            Debug.Log($"Layer In!:{col.gameObject.name}");
        }
    }
}
