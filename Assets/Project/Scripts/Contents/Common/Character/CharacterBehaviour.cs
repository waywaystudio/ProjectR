using System.Collections.Generic;
using MainGame;
using UnityEngine;

namespace Common.Character
{
    public class CharacterBehaviour : MonoBehaviour
    {
        [SerializeField] string characterName = string.Empty;
        [SerializeField] private int characterID;
        [SerializeField] private string characterClass;
        [SerializeField] private List<string> equipmentList;
        [SerializeField] private List<string> extraList;

        public void Initialize(string characterName)
        {
            // var characterProfile = MainData.
            
            // If(!MainData.TryGetCharacter(characterName, out cb)) debugError & Return;
            
            // Field
            // name
            // ID
            // class
            // equipment
            // spineImage (race + equipment maybe...)
            // extraStatus;
        }
    }
}
