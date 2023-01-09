using Core;
using MainGame;
using UnityEngine;

namespace Character
{
    public class AdventurerBehaviour : CharacterBehaviour
    {
        [SerializeField] private DataIndex combatClassID;
        [SerializeField] private string role;

        public DataIndex CombatClassID => combatClassID;
        public string Role => role;


#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();
            
            var profile = MainData.GetAdventurer(characterName.ToEnum<DataIndex>());

            dataIndex = (DataIndex)profile.ID;
            role = profile.Role;
            combatClassID = (DataIndex)profile.CombatClassId;
        }
#endif
    }
}
