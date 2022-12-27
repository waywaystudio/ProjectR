using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace Common.Character.Monster
{
    [TaskCategory("Monster")]
    public class MonsterMoveBehavior : Action
    {
        private WillBeChangedMonsterBehavior mb;

        public override void OnAwake()
        {
            mb = GetComponent<WillBeChangedMonsterBehavior>();
        }

        public override TaskStatus OnUpdate()
        {
            var posX = Random.Range(-10f, 10f);
            var posZ = Random.Range(-10f, 10f);

            var destination = new Vector3(posX, 0f, posZ);
            // mb.Walk(destination);
            
            return TaskStatus.Success;
        }
    }
}
