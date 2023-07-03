using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public interface ISearchable : IObjectName
    {
        // + string Name { get; }
        // + GameObject gameObject { get; }
        // + Vector3 Position { get; }
        
        Dictionary<int, List<GameObject>> SearchedTable { get; }
    }
}
