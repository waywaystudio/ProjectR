using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Databases
{
    public abstract class DataObject : ScriptableObject
    {
        public DataIndex Index;
        public abstract List<IIdentifier> KeyList { get; }
        public abstract T Get<T>(DataIndex dataIndex) where T : class, IIdentifier;
        public abstract T Get<T>(int idCode) where T : class, IIdentifier;
    }

    public class DataObject<T> : DataObject where T : class, IIdentifier 
    {
        [SerializeField] protected List<T> list;
        
        private List<IIdentifier> keyList = new();
        private Dictionary<int, T> table;

        protected List<T> List { get => list; set => list = value; }

        public override List<IIdentifier> KeyList => keyList ??= list.ConvertAll(element => (IIdentifier)element);
        public Dictionary<int, T> Table => table ??= list.ToDictionary(x => x.ID);

        public override T0 Get<T0>(DataIndex dataIndex)
        {
            if (Table.TryGetValue((int)dataIndex, out var value))
                return value as T0;

            Debug.LogError($"Can't Find {dataIndex} from {name} object.");
            return null;
        }
        
        public override T0 Get<T0>(int idCode)
        {
            if (Table.TryGetValue(idCode, out var value)) 
                return value as T0;
            
            Debug.LogError($"Can't Find {idCode} from {name} object.");
            return null;
        }
    }
}
