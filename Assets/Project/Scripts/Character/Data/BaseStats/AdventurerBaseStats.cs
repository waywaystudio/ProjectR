using MainGame;

namespace Common.Character.Data.BaseStats
{
    public class AdventurerBaseStats : ClassBaseStats
    {
#if UNITY_EDITOR
        protected override void SetUp()
        {
            var adventurerBehaviour = GetComponentInParent<AdventurerBehaviour>();
            
            baseStatCode = adventurerBehaviour.CombatClassID;
            var classData = MainData.GetCombatClass(baseStatCode);

            maxHp = classData.MaxHp;
            moveSpeed = classData.MoveSpeed;
            maxResource = classData.MaxResource;
            critical = classData.Critical;
            haste = classData.Haste;
            hit = classData.Hit;
            evade = classData.Evade;
            armor = classData.Armor;
        }
#endif
    }
}
