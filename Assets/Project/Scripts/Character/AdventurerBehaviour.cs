using Core;
using MainGame;
using UnityEngine;

namespace Character
{
    public class AdventurerBehaviour : CharacterBehaviour
    {
        [SerializeField] private DataIndex combatClassID;

        public DataIndex CombatClassID => combatClassID;
        


#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();
            
            var profile = MainData.GetAdventurer(characterName.ToEnum<DataIndex>());

            dataIndex = (DataIndex)profile.ID;
            role = profile.Role.ToEnum<RoleType>();
            combatClassID = (DataIndex)profile.CombatClassId;
        }
#endif
    }
}
