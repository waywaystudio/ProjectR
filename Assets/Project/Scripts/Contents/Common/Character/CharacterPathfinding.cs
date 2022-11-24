using System;
using System.Linq;
using Pathfinding;
using UnityEngine;

namespace Common.Character
{
    public class CharacterPathfinding : MonoBehaviour
    {
        private AIMove aiMove;
        private Seeker agent;
        private Action onTargetReached;
        private Transform rootObject;
        private Vector3 tempDirection;

        public bool HasPath => aiMove.hasPath;
        public bool IsFinished => aiMove.reachedEndOfPath;
        public Vector3 Destination
        {
            get => aiMove.destination;
            set => aiMove.destination = value; 
        }

        public void Initialize()
        {
            // fleePath에 여러 값을 캐릭터 성향으로 결정할지, 영향범위에 따라서 변동하는 값일지 결정해야 한다.
            // fleePath의 모든 값을 캐릭터 성향으로 사용할 필요는 없고 한 가지 정도만 남기는 것도 방법이다.
            // 성향으로 사용된다면, Initialize 파라메타로 짜 주어야 한다.
            
            
            // aiMove.Initialize(onTargetReached);
        }
        
        public void Move(Vector3 destination, float speed)
        {
            var abPath = ABPath.Construct(rootObject.position, destination);

            // aiMove.rea
            
            aiMove.maxSpeed = speed;
            agent.StartPath(abPath);
        }

        public void MoveAround(Vector3 position, int spreadRange, float speed)
        {
            var constPath = ConstantPath.Construct(position, spreadRange);
            var destination = constPath.vectorPath.Last();

            Move(destination, speed);
        }

        public void Flee(Vector3 from, int fleeLength, float aimStrength, int spread, float speed)
        {
            // 현재는 input position 으로부터 무조건 도망친다.
            // 실제로 input은 position 뿐만 아니라, 영향범위가 있기 때문에
            // 영향범위 밖이면, Avoid 하지 않아도 된다.
            // 그 판단을 여기서 해줄지, 아니면 다른 곳에서 하고 이 함수는 간단하게 짤지 결정해야 한다.
            // 가급적 여기서 판단하지 말고 간단하게 끝내자.
            var fleePath = FleePath.Construct(rootObject.position, from, fleeLength);

            aimStrength = Mathf.Clamp01(aimStrength);
            fleePath.aimStrength = aimStrength;
            fleePath.spread = spread;

            aiMove.maxSpeed = speed;
            agent.StartPath(fleePath);
        }

        public void Stop()
        {
            // Destination = rootObject.position;
            // 위 경우에 Flip 덜덜이 발생함.
            Destination = aiMove.steeringTarget;
        }
        

        private void Awake()
        {
            agent ??= GetComponent<Seeker>();
            aiMove ??= GetComponent<AIMove>();
            rootObject = aiMove.RootObject.transform;
        }
    }
}
