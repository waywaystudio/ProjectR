using System;
using System.Collections.Generic;
using MainGame;
using UnityEngine;

namespace Common.Character
{
    public class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField] private string characterName = string.Empty;
        [SerializeField] private int id;
        [SerializeField] private string combatClass;
        [SerializeField] private string role;
        [SerializeField] private float baseAttackSpeed;
        [SerializeField] private float baseRange;
        
        public string CharacterName => characterName;
        public int ID => id;
        public string CombatClass => combatClass;
        public string Role => role;

        public float AttackSpeed
        {
            get => baseAttackSpeed;
            set => baseAttackSpeed = value;
        }

        public float Range
        {
            get => baseRange;
            set => baseRange = value;
        }
        

        public void Initialize(string characterName)
        {
            var profile = MainData.GetAdventurerData(characterName);

            this.characterName = profile.Name;
            id = profile.ID;
            combatClass = profile.Job;
            role = profile.Role;

            var classData = MainData.GetCombatClassData(combatClass);

            baseAttackSpeed = classData.AttackSpeed;
            baseRange = classData.Range;
        }


        private void Awake()
        {
            Initialize("Kungen");
        }
    }
}
