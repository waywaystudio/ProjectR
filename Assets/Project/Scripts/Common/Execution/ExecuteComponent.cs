using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Execution
{
    public abstract class ExecuteComponent : MonoBehaviour
    {
        [SerializeField] protected ExecuteType type;
        [SerializeField] protected ExecuteGroup group = ExecuteGroup.Group1;

        private IExecutable executor;

        public ExecuteType Type => type;
        public ExecuteGroup Group => group;

        protected IExecutable Executor 
            => executor 
            ??= GetComponent<IExecutable>() // TODO. 추후에 Parent 구조로 모두 변경.
            ?? GetComponentInParent<IExecutable>();

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

/* Annotation 
 * 대부분의 Execution은 public void Execution(ICombatTaker taker) 을 사용한다. 
 * StatusEffect 는 instantMultiplier를 거의 사용하지 않기 때문에, 깡통 함수이다.
 * 이러한 디자인에 대해서는 다음을 참고했다.
 *
 * <<Q>>
 * i have a programming structure issue.
 * there three classes which are inherit relation.
 * class "A" is parent, and "a1", "a2" are children.
 * in other class "B", has a List<A> field.
 * and "B"class has a function that iterate List<A> field.
 * problem is here. on during iterating,
 * if A is a1, i want to run a1's personal function.
 * so in this code, downCasting might be occured.
 * if i designed A class include a1's personal function by virtual or abstract, a2 class has that but may be empty function.
 * so which is better way in this situation?
 * create abstract or virtual class although a2 implement empty? or downCasting on B class.
 *
 * <<A>>
 * In this situation, I would recommend using a virtual or abstract method
 * in class A and providing an empty implementation for class a2 if needed.
 * This approach is more object-oriented and takes advantage of polymorphism,
 * which makes the code more maintainable and easier to extend in the future.
 * Using downCasting in class B could work, but it may result in a less maintainable and less flexible design.
 * DownCasting can introduce potential runtime errors if you don't check for the correct types,
 * and it makes the code harder to understand and modify. */