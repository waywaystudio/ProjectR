using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Execution
{
    [Serializable]
    public class TakerExecutor 
    {
        [SerializeField] protected Table<ExecuteGroup, TakerExecute> takerExecutionTable;
        
        /// <summary>
        /// For Compact Action
        /// </summary>
        public void ToTaker(ICombatTaker taker) => ToTaker(taker, ExecuteGroup.Group1);
        public void ToTaker(ICombatTaker taker, ExecuteGroup group)
        {
            takerExecutionTable[group]?.List.ForEach(exe => exe.Execution(taker));
        }
        
        [Serializable] protected class TakerExecute : CombatExecutionList<TakerExecution>
        {
            public TakerExecute(TakerExecution component) => List = new List<TakerExecution> { component };
        }
        
        
#if UNITY_EDITOR
        public void EditorGetExecutions(GameObject parent)
        {
            var takerExecuteList = parent.GetComponentsInChildren<TakerExecution>();
            
            takerExecutionTable.Clear();
            
            takerExecuteList.ForEach(exe =>
            {
                if (!takerExecutionTable.ContainsKey(exe.Group))
                {
                    takerExecutionTable.Add(exe.Group, new TakerExecute(exe));
                }
                else
                {
                    takerExecutionTable[exe.Group].List.AddUniquely(exe);
                }
            });
        }
#endif
    }
}
