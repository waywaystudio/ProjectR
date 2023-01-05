using MainGame;

namespace Character.Data.BaseStats
{
    public class AdventurerConstStats : CharacterConstStats
    {
#if UNITY_EDITOR
        public override void SetUp()
        {
            base.SetUp();
            
            var adventurerBehaviour = GetComponentInParent<AdventurerBehaviour>();
            
            baseStatCode = adventurerBehaviour.CombatClassID;
            var classData = MainData.GetCombatClass(baseStatCode);

            maxHp.Value       = classData.MaxHp;
            moveSpeed.Value   = classData.MoveSpeed;
            maxResource.Value = classData.MaxResource;
            critical.Value    = classData.Critical;
            haste.Value       = classData.Haste;
            hit.Value         = classData.Hit;
            evade.Value       = classData.Evade;
            armor.Value      = classData.Armor;
        }
#endif
    }
}
