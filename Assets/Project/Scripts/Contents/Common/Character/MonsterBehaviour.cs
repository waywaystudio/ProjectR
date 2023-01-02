using Core;
using MainGame;

namespace Common.Character
{
    public class MonsterBehaviour : CharacterBehaviour
    {
        private const string TestKey = "TestKey";
        
        protected override void Start()
        {
            StatTable.Register(StatCode.AddMoveSpeed, TestKey.GetHashCode(), 20f, true);
            StatTable.Register(StatCode.AddMaxHp, TestKey.GetHashCode(), 3000f, true);
            
            base.Start();
        }
        
        private void OnDisable()
        {
            StatTable.Unregister(StatCode.AddMoveSpeed, TestKey.GetHashCode());
            StatTable.Unregister(StatCode.AddMaxHp, TestKey.GetHashCode());
        }
        
#if UNITY_EDITOR
        protected override void SetUp()
        {
            var profile = MainData.GetBoss(characterName.ToEnum<IDCode>());

            id = (IDCode)profile.ID;
        }
#endif
    }
}
