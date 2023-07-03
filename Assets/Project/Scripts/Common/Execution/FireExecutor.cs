using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Execution
{
    [Serializable]
    public class FireExecutor
    {
        [SerializeField] protected Table<ExecuteGroup, FireExecute> fireExecutionTable;

        /// <summary>
        /// For Compact Action
        /// </summary>
        public void ToPosition(Vector3 position) => ToPosition(position, ExecuteGroup.Group1);
        public void ToPosition(Vector3 position, ExecuteGroup group)
        {
            fireExecutionTable[group]?.List.ForEach(exe => exe.Execution(position));
        }
        
        [Serializable] protected class FireExecute : CombatExecutionList<FireExecution>
        {
            public FireExecute(FireExecution component) => List = new List<FireExecution> { component };
        }
        
        
#if UNITY_EDITOR
        public void EditorGetExecutions(GameObject parent)
        {
            var fireExecuteList = parent.GetComponentsInChildren<FireExecution>();
            
            fireExecutionTable.Clear();

            fireExecuteList.ForEach(exe =>
            {
                if (!fireExecutionTable.ContainsKey(exe.Group))
                {
                    fireExecutionTable.Add(exe.Group, new FireExecute(exe));
                }
                else
                {
                    fireExecutionTable[exe.Group].List.AddUniquely(exe);
                }
            });
        }
#endif
    }
}
