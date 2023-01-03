using Core;
using MainGame;
using UnityEngine;

namespace Common.Character
{
    public class AdventurerBehaviour : CharacterBehaviour
    {
        [SerializeField] private IDCode combatClassID;
        [SerializeField] private string role;

        public IDCode CombatClassID => combatClassID;
        public string Role => role;


#if UNITY_EDITOR
        protected override void SetUp()
        {
            var profile = MainData.GetAdventurer(characterName.ToEnum<IDCode>());

            id = (IDCode)profile.ID;
            role = profile.Role;
            combatClassID = (IDCode)profile.CombatClassId;
        }
#endif
    }
}
