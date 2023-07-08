using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Execution
{
    [Serializable]
    public class Executor
    {
        // [SerializeField] private List<TakerExecution> hitExecutionList; 
        // [SerializeField] private List<TakerExecution> subHitExecutionList; 
        // [SerializeField] private List<FireExecution> fireExecutionList;
        // [SerializeField] private List<FireExecution> subFireExecutionList;
        
        [SerializeField] private Table<ExecuteGroup, TakerExecute> takerExecutionTable;
        [SerializeField] private Table<ExecuteGroup, FireExecute> fireExecutionTable;

        /// <summary>
        /// For Compact Action
        /// </summary>
        public void ToTaker(ICombatTaker taker) => ToTaker(taker, ExecuteGroup.Main);
        public void ToTaker(ICombatTaker taker, ExecuteGroup group)
        {
            takerExecutionTable[group]?.List.ForEach(exe => exe.Hit(taker));
        }
        
        public void ToPosition(Vector3 position, ExecuteGroup group = ExecuteGroup.Main)
        {
            fireExecutionTable[group]?.List.ForEach(exe => exe.Fire(position));
        }


        [Serializable] private class CombatExecute<T>
        {
            public List<T> List;

            public CombatExecute() => List = new List<T>();
            public CombatExecute(T component) => List = new List<T> { component };

            public void Add(T exe) => List.AddUniquely(exe);
            public void Remove(T exe) => List.RemoveSafely(exe);
            public void Clear() => List.Clear();
        }

        [Serializable] private class TakerExecute : CombatExecute<HitExecution>
        {
            public TakerExecute(HitExecution component) => List = new List<HitExecution> { component };
        }
        [Serializable] private class FireExecute : CombatExecute<FireExecution>
        {
            public FireExecute(FireExecution component) => List = new List<FireExecution> { component };
        }


#if UNITY_EDITOR
        public void EditorGetExecutions(GameObject parent)
        {
            var takerExecuteList = parent.GetComponentsInChildren<HitExecution>();
            var fireExecuteList = parent.GetComponentsInChildren<FireExecution>();
            
            takerExecutionTable.Clear();
            fireExecutionTable.Clear();
            
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
