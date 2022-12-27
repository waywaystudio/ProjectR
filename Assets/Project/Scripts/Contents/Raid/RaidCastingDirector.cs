using System;
using System.Collections.Generic;
using System.Linq;
using Common.Character;
using UnityEngine;

namespace Raid
{
    public class RaidCastingDirector : MonoBehaviour
    {
        [SerializeField] private List<AdventurerBehaviour> adventurerList;
        [SerializeField] private List<MonsterBehaviour> monsterList;

        public Dictionary<int, AdventurerBehaviour> AdventurerTable { get; } = new();
        public Dictionary<int, MonsterBehaviour> MonsterTable { get; } = new();

        // public void SpawnCharacter()
        // private void SpawnAdventurer()
        // private void SpawnMonster()

        private void Awake()
        {
            adventurerList.ForEach(x => AdventurerTable.Add(x.ID, x));
            monsterList.ForEach(x => MonsterTable.Add(x.ID, x));
        }

        private void GetList()
        {
            adventurerList = GetComponentsInChildren<AdventurerBehaviour>().ToList();
            monsterList = GetComponentsInChildren<MonsterBehaviour>().ToList();
        }
    }
}

// 캐스팅 디렉터의 모험가 리스트를 바탕으로, 전역UI를 그려보자.
// 전역 UI는 정보와 무관하게 일단 고유 자리와 오브젝트를 가지고 있을 것이다.
// CastingDirector의 정보를 받아, 초상화, 아이콘 Hp, Resource등을 표기하는데
// 실시간 전투와 동기화 되어야 하므로, 단순히 Awake에서 Value를 받아 올 수는 없고
// CharacterBehavior를 연결시켜주어야 한다.
// PartiUI는 CB의 Hp와 Resource에 OnValueChanged ActionTable에 Register하여 UI Bar를 채우고 줄이고 할 것이다.
// 파티원을 클릭하면, 하단의 기술 Bar가 변하고, 카메라가 Transform을 쫓아야 한다.
// 따라서 Combating을 추적해야 하고, Transform을 알아야 한다.
