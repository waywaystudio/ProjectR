using System.Collections.Generic;
using Common.Characters;
using Serialization;
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
        
        public List<CharacterBehaviour> GetAllCharacters() => characterList;
        public CharacterBehaviour GetNextCharacter(CombatClassType currentType) => characterList.GetNext(Table[currentType]);
        public CharacterBehaviour GetPreviousCharacter(CombatClassType currentType) => characterList.GetPrevious(Table[currentType]);

        // public void Save()
        // {
        //     characterDataList.ForEach(data => data.Save());
        // }
        //
        // public void Load()
        // {
        //     characterDataList.ForEach(data => data.Load());
        // }


        private CharacterBehaviour GetCharacterByType(CombatClassType currentType)
        {
            CharacterBehaviour currentCharacter = null; 
            
            foreach (var character in characterList)
            {
                if (character.CombatClass == currentType) 
                    currentCharacter = character;
            }

            return currentCharacter;
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
