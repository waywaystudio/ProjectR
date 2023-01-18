using Core;
using MainGame;
using UnityEngine;

namespace Character
{
    public class MonsterBehaviour : CharacterBehaviour
    {
#if UNITY_EDITOR
        public override void SetUp()
        {
            var profile = MainData.GetBoss(characterName.ToEnum<DataIndex>());

            role      = RoleType.Boss;
            dataIndex = (DataIndex)profile.ID;
        }
#endif
    }
}

// protected override void Update()
// {
//     base.Update();
//     
//     // TEMP. 좌클릭 이벤트가 들어왔다면
//     if (Input.GetMouseButtonUp(0))
//     {
//         var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//     
//         // 레이저가 뭔가에 맞았다면
//         if (Physics.Raycast(ray, out var raycastHit))
//         {
//             // 맞은 위치를 목적지로 저장
//             Run(new Vector3(raycastHit.point.x, 0f, raycastHit.point.z));
//         }
//     }
// }
