using System;
using Common.Characters;

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
    }

    public class EthosTaskProgression
    {
        public Func<float> Progress { get; }
        public Func<float> Max { get; }

        public EthosTaskProgression(Func<float> refProgress, Func<float> refMax)
        {
            Progress = refProgress;
            Max      = refMax;
        }
        
        public EthosTaskProgression(Func<float> refProgress, float constMax)
        {
            Progress = refProgress;
            Max += () => constMax;
        }

        public float NormalizedProgression
        {
            get
            {
                if (Max is null || Max.Invoke() == 0f) return 0f;

                var progress = Progress is null ? 0f : Progress.Invoke();
                
                return Math.Abs(progress / Max.Invoke());
            }
        }

        public bool IsComplete => Math.Abs(NormalizedProgression - 1f) < 0.0001f;
    }

    public class EthosTaskBoolean
    {
        public bool IsComplete { get; set; }
    }
}
