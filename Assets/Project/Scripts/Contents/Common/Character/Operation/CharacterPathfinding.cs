using Pathfinding;
using UnityEngine;

namespace Common.Character
{
    public class CharacterPathfinding : MonoBehaviour
    {
        [SerializeField] private CharacterBehaviour cb;
        
        private AIMove aiMove;
        private Seeker agent;

        public bool HasPath => aiMove.hasPath;
        public bool IsReached => aiMove.reachedEndOfPath;

        public void Move(Vector3 destination)
        {
            var abPath = ABPath.Construct(cb.transform.position, destination);

            aiMove.maxSpeed = cb.MoveSpeed;
            agent.StartPath(abPath);
        }

        public void Stop()
        {
            // Destination = rootObject.position;
            // 위 경우에 Flip 덜덜이 발생함.
            aiMove.destination = aiMove.steeringTarget;
        }
        

        private void Awake()
        {
            agent = GetComponent<Seeker>();
            aiMove = GetComponent<AIMove>();
        }
    }
}


// public void Initialize()
// {
// fleePath에 여러 값을 캐릭터 성향으로 결정할지, 영향범위에 따라서 변동하는 값일지 결정해야 한다.
// fleePath의 모든 값을 캐릭터 성향으로 사용할 필요는 없고 한 가지 정도만 남기는 것도 방법이다.
// 성향으로 사용된다면, Initialize 파라메타로 짜 주어야 한다.
// aiMove.Initialize(onTargetReached);
// }
// public void MoveAround(Vector3 position, int spreadRange, float speed)
// {
//     var constPath = ConstantPath.Construct(position, spreadRange);
//     var destination = constPath.vectorPath.Last();
//
//     Move(destination, speed);
// }
//
// public void Flee(Vector3 from, int fleeLength, float aimStrength, int spread, float speed)
// {
//     // 현재는 input position 으로부터 무조건 도망친다.
//     // 실제로 input은 position 뿐만 아니라, 영향범위가 있기 때문에
//     // 영향범위 밖이면, Avoid 하지 않아도 된다.
//     // 그 판단을 여기서 해줄지, 아니면 다른 곳에서 하고 이 함수는 간단하게 짤지 결정해야 한다.
//     // 가급적 여기서 판단하지 말고 간단하게 끝내자.
//     var fleePath = FleePath.Construct(cb.transform.position, from, fleeLength);
//
//     aimStrength = Mathf.Clamp01(aimStrength);
//     fleePath.aimStrength = aimStrength;
//     fleePath.spread = spread;
//
//     aiMove.maxSpeed = speed;
//     agent.StartPath(fleePath);
// }