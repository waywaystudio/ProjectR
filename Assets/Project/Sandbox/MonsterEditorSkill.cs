using Character;
using Common;
using Common.Character;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

public class MonsterEditorSkill : MonoBehaviour
{
    [SerializeField] private LayerMask playerLayer;
    private SphereCollider searchCollider;
    private readonly Collider[] buffer = new Collider[50];
    
    [Button]
    private void TempDamage()
    {
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position, 
            200f, buffer, playerLayer);

        buffer.ForEach(x =>
        {
            if (x.IsNullOrEmpty()) return;
            x.TryGetComponent(out CharacterBehaviour taker);

            taker.DynamicStatEntry.Hp.Value -= 200f;
        });
    }
}
