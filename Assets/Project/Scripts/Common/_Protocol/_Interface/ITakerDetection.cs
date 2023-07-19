using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public interface ITakerDetection
    {
        List<ICombatTaker> GetTakers(Vector3 pivot, Vector3 forward, LayerMask layerMask, SizeEntity entity);
    }
}
