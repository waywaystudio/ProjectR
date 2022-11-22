using System;
using Pathfinding;
using UnityEngine;

namespace Common.Character
{
    public class CharacterPathfinding : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private int fleeLength = 5000;
        [SerializeField] private int fleeSpread = 500;
        [SerializeField] private float fleeAimStrength = 1f;

        private AIMove aiMove;
        private Seeker agent;
        private Action onTargetReached;
        private Transform rootObject;

        public bool CanMove { get => aiMove.canMove; set => aiMove.canMove = value; }
        public float MoveSpeed => aiMove.maxSpeed;
        public Vector3 Direction => (aiMove.steeringTarget - rootObject.position).normalized;

        private Vector3 Destination { set => aiMove.destination = value; }

        public void Initialize(float moveSpeed, Action onTargetReached)
        {
            // fleePath에 여러 값을 캐릭터 성향으로 결정할지, 영향범위에 따라서 변동하는 값일지 결정해야 한다.
            // fleePath의 모든 값을 캐릭터 성향으로 사용할 필요는 없고 한 가지 정도만 남기는 것도 방법이다.
            // 성향으로 사용된다면, Initialize 파라메타로 짜 주어야 한다.

            this.moveSpeed = moveSpeed;
            aiMove.Initialize(onTargetReached);
        }

        public void Move(Vector3 destination)
        {
            Destination = destination;
        }

        public void Stop()
        {
            // Destination = rootObject.position;
            // 위 경우에 Flip 덜덜이 발생함.
            Destination = aiMove.steeringTarget;
        }

        public void Flee(Vector3 from)
        {
            // 현재는 input position 으로부터 무조건 도망친다.
            // 실제로 input은 position 뿐만 아니라, 영향범위가 있기 때문에
            // 영향범위 밖이면, Avoid 하지 않아도 된다.
            // 그 판단을 여기서 해줄지, 아니면 다른 곳에서 하고 이 함수는 간단하게 짤지 결정해야 한다.
            // 가급적 여기서 판단하지 말고 간단하게 끝내자.

            var fleePath = FleePath.Construct(rootObject.position, from, fleeLength);
            
            fleePath.aimStrength = fleeAimStrength;
            fleePath.spread = fleeSpread;

            agent.StartPath(fleePath);
        }

        public void Scrum(Vector3 center)
        {
            // 진형 이동할 일이 있다면 사용할 수도 있다.
        }

        private void Awake()
        {
            agent ??= GetComponent<Seeker>();
            aiMove ??= GetComponent<AIMove>();
            aiMove.maxSpeed = moveSpeed;
            rootObject = aiMove.RootObject.transform;
        }
    }
}
