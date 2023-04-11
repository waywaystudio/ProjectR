using System;
using System.Collections.Generic;
using Common.Characters;
using UnityEngine;

namespace Common.PlayerCamps
{
    public class CharacterManager : MonoBehaviour, IEditable
    {
        [SerializeField] private List<CharacterBehaviour> characterList;

        private readonly Dictionary<CombatClassType, CharacterBehaviour> table = new();
        private Dictionary<CombatClassType, CharacterBehaviour> Table
        {
            get
            {
                if (table.IsNullOrEmpty())
                    characterList.ForEach(character => table.Add(character.CombatClass, character));
                
                return table;
            }
        }


        public CharacterBehaviour Get(CombatClassType type)
        {
            if (!Table.TryGetValue(type, out var character))
            {
                Debug.LogError($"Can't Find Character. Input Type:{type.ToString()}");
                return null;
            }
            
            character.Initialize();

            return character;
        }
        
        public CharacterBehaviour NextCharacter(CombatClassType currentType)
        {
            CharacterBehaviour currentCharacter = null; 
            
            foreach (var character in characterList)
            {
                if (character.CombatClass == currentType) 
                    currentCharacter = character;
            }

            return currentCharacter == null ? null : characterList.GetNext(currentCharacter);
        }


        private void Awake()
        {
            characterList.Sort((a, b) => a.CombatClass.CompareTo(b.CombatClass));
            characterList.ForEach(character => character.Initialize());
            // characterList.ForEach(character => Table.Add(character.CombatClass, character));
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            GetComponentsInChildren(true, characterList);
            
            characterList.Sort((a, b) => a.CombatClass.CompareTo(b.CombatClass));
        }
#endif
    }
}
