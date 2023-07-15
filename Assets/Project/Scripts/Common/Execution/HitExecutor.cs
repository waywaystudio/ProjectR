using System;
using System.Collections.Generic;
using Common.Execution.Hits;
using UnityEngine;

namespace Common.Execution
{
    [Serializable]
    public class HitExecutor 
    {
        [SerializeField] private List<HitExecution> hitExecutionList;

        public void Initialize(ICombatObject combatObject)
        {
            hitExecutionList.ForEach(exe => exe.Initialize(combatObject));
            
            var builder = new CombatSequenceBuilder(combatObject.Sequence);
            var mainHit = hitExecutionList.FindAll(exe => exe.Group == ExecuteGroup.Main);
            var subHit = hitExecutionList.FindAll(exe => exe.Group  == ExecuteGroup.Sub);
            
            if (mainHit.Count > 0) builder.AddHit("HitExecution", taker => mainHit.ForEach(exe => exe.Hit(taker)));
            if (subHit.Count  > 0) builder.AddSubHit("SubHitExecution", taker => subHit.ForEach(exe => exe.Hit(taker)));
        }

        public T OfType<T>() where T : HitExecution
        {
            foreach (var item in hitExecutionList)
            {
                if (item is T typedItem)
                {
                    return typedItem;
                }
            }

            Debug.LogWarning($"Can't Find {typeof(T).Name} in execution list.");
            return null;
        }

        
        
#if UNITY_EDITOR
        public void GetExecutionInEditor(Transform parent)
        {
            parent.GetComponentsInChildren(hitExecutionList);
        }
#endif
    }
}
