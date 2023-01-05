using MainGame;

namespace Character.Data.BaseStats
{
    public class MonsterConstStats : CharacterConstStats
    {
#if UNITY_EDITOR
        public override void SetUp()
        {
            var monsterBehaviour = GetComponentInParent<MonsterBehaviour>();
            
            baseStatCode = monsterBehaviour.ID;
            var bossData = MainData.GetBoss(baseStatCode);

            maxHp = bossData.MaxHp;
            moveSpeed = bossData.MoveSpeed;
            maxResource = bossData.MaxResource;
            critical = bossData.Critical;
            haste = bossData.Haste;
            hit = bossData.Hit;
            evade = bossData.Evade;
            armor = bossData.Armor;
        }
#endif
    }
}
