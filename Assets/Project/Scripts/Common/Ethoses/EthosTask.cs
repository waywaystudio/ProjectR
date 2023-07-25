using System;

namespace Common.Ethoses
{
// ### Equip Condition
//
//     - 역할 군 유무
//         - 캐릭터 레벨
//
// ### Variant Idea Sketch
//
//         - No damage
//         - 체력이 n % 이하로 내려가지 않은 채로 클리어 - OnCombatTaken
//         - 보스 클리어 n회 - BattleLog
//         - xx 스킬 n회 사용 - Venturer.SkillTable[SkillIndex].Add("AddCount")
//         - xx 스킬로 n 데미지 - BattleLog[CombatClass.Warrior][DataIndex.XX].Value
//         - xx 스킬로 n 치유
//         - 한 스테이지에서 -
//         - 총 이동 거리 n 미터 ??
//         - n분 안에 클리어 - BattleLog
//         - vinion n명 죽임
//         - 특정 보스 클리어
//         - 피해량 1위 
//         - (전사) 생명력 추출로 50 % 이상의 체력 회복 
//         - (전사) 도약 공격으로 n미터 이상 이동 공격
//         - (전사) 마무리 일격을 사용하기 전에 분노 기술 10회 이상 사용
    
    [Serializable]
    public class EthosTask
    {
        public ConditionTable ConditionTable { get; set; } = new();

        public void TaskInitialize()
        {
            
        }

        public EthosTask CreateInstance(int campLevel)
        {
            
            
            return null;
        }

        private bool NoDamageCondition(float damagedAmount)
        {
            return damagedAmount <= 0f;
        }

        private bool UnderHealthProportionCondition(in float proportion, float damagedAmount)
        {
            return damagedAmount <= proportion;
        }
    }

    /*
     * NoDamage Condition을 확인하려면 데미지 미터Log가 필요할 듯 하다.
     */
    public class NoDamage
    {
        // character.OnCombatProvided
        public bool IsSuccess { get; set; }

        public void AssignCondition(CombatEntity combatEntity) { }
    }

    public class UnderHealthProportion
    {
        public bool IsSuccess { get; set; } = true;
        public float Standard { get; set; }

        public UnderHealthProportion(float standard) => Standard = standard;  

        public void AssignCondition(CombatEntity combatEntity)
        {
            var providerHealth = combatEntity.Provider.Hp.Value;
            var maxHealth = combatEntity.Provider.StatTable.MaxHp;
            var currentProportion = providerHealth / maxHealth;

            if (currentProportion <= Standard) IsSuccess = false;
        }
    }

    /*
     * DefeatVillain을 확인하려면 Raid Sequencer가 필요할 듯 하다.
     */
    public class DefeatVillain
    {
        public bool IsSuccess { get; set; } = true;

        public void AssignCondition()
        {
            
        }
    }
    
    // xx 스킬 n회 사용
    public class UseSkillInSingleBattle
    {
        public bool IsSuccess { get; set; } = true;
        public float AccumulateCount { get; set; }
        public DataIndex TargetSkill { get; set; }

        public UseSkillInSingleBattle(DataIndex dataIndex, int count)
        {
            TargetSkill     = dataIndex;
            AccumulateCount = count;
        } 

        public void AssignCondition(CombatEntity combatEntity)
        {
            if (combatEntity.CombatIndex != TargetSkill) return;
        }
    }

    public class UseSkillInStackBattle
    {
        public bool IsSuccess { get; set; } = true;
        public float AccumulateCount { get; set; }
        public DataIndex TargetSkill { get; set; }
        
        public UseSkillInStackBattle(DataIndex dataIndex) => TargetSkill = dataIndex; 

        public void AssignCondition(CombatEntity combatEntity)
        {
            if (combatEntity.CombatIndex != TargetSkill) return;
        }
    }
}
