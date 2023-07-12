using System;
using System.Collections.Generic;
using Common.Execution.Fires;
using UnityEngine;

namespace Common.Execution
{
    [Serializable]
    public class FireExecutor
    {
        [SerializeField] private List<FireExecution> fireExecutionList;

        public void Initialize(CombatSequence sequence, IActionSender sender)
        {
            if (fireExecutionList.IsNullOrEmpty()) return;
            
            fireExecutionList.ForEach(exe => exe.Initialize(sender));
            
            var builder = new CombatSequenceBuilder(sequence);
            var mainFire = fireExecutionList.FindAll(exe => exe.Group == ExecuteGroup.Main);
            var subFire = fireExecutionList.FindAll(exe => exe.Group  == ExecuteGroup.Sub);
            
            if (mainFire.Count > 0) builder.AddFire("FireExecution", position => mainFire.ForEach(exe => exe.Fire(position)));
            if (subFire.Count  > 0) builder.AddSubFire("SubFireExecution", position => subFire.ForEach(exe => exe.Fire(position)));
        }
        
        public T OfType<T>() where T : FireExecution
        {
            foreach (var item in fireExecutionList)
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
            parent.GetComponentsInChildren(fireExecutionList);
        }
#endif
    }
}
