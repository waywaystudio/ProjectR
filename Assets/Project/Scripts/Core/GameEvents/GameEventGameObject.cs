using UnityEngine;

namespace Core.GameEvents
{
    [CreateAssetMenu(menuName = "ScriptableObject/GameEvent/GameObject")]
    public class GameEventGameObject : GameEvent<UnityEngine.GameObject> {}
}