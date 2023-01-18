using System.Collections.Generic;
using Character;
using UnityEngine;

namespace Scene.Raid
{
    public class RaidCastingDirector : MonoBehaviour
    {
        [SerializeField] private List<AdventurerBehaviour> adventurerList = new();
        [SerializeField] private List<MonsterBehaviour> monsterList = new();

        public List<AdventurerBehaviour> AdventurerList => adventurerList;
        public List<MonsterBehaviour> MonsterList => monsterList;


        private void GetList()
        {
            AdventurerList.AddRange(GetComponentsInChildren<AdventurerBehaviour>());
            MonsterList.AddRange(GetComponentsInChildren<MonsterBehaviour>());
        }
    }
}