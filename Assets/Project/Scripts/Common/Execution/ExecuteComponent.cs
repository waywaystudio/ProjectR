using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Execution
{
    public abstract class ExecuteComponent : MonoBehaviour
    {
        [SerializeField] protected ExecuteGroup group = ExecuteGroup.Group1;

        private IOriginalProvider origin;
        public ExecuteGroup Group => group;

        protected IOriginalProvider Origin => origin ??= GetComponentInParent<IOriginalProvider>();

        public abstract void Execution(ICombatTaker taker);
        public virtual void Execution(Vector3 trapPosition) { }
    }

    [Serializable] 
    public class Executions
    {
        public List<ExecuteComponent> ExecutionList;

        public Executions()
        {
            ExecutionList = new List<ExecuteComponent>();
        }
        public Executions(ExecuteComponent component)
        {
            ExecutionList = new List<ExecuteComponent> { component };
        }

        public void Add(ExecuteComponent exe) => ExecutionList.AddUniquely(exe);
        public void Remove(ExecuteComponent exe) => ExecutionList.RemoveSafely(exe);
        public void Clear() => ExecutionList.Clear();
    }
}