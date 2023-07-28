using System.Collections.Generic;
using System.Linq;
using Common.Runes.Rewards;
using Common.Runes.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Runes
{
    [CreateAssetMenu(menuName = "ScriptableObject/RuneCreator", fileName = "EthosRuneCreator")]
    public class RuneCreator : ScriptableObject
    {
        [SerializeField] private List<RewardRuneWeight> rewardRuneWeights;
        [SerializeField] private List<TaskRuneWeight> taskRuneWeights;

        private static DataIndex GetRandomSkill
        {
            get
            {
                var skillEntry = new List<DataIndex>
                {
                    DataIndex.Slash,
                    DataIndex.Bash,
                    DataIndex.Smash,
                    DataIndex.LeapAttack,
                    DataIndex.Deathblow,
                    DataIndex.BloodSmash,
                    DataIndex.Exploitation,
                    DataIndex.Finisher,
                };

                return skillEntry.Random();
            }
        }

        private TaskRune CreateTaskRune()
        {
            var totalWeights = taskRuneWeights.Sum(rune => rune.Weight);
            var randomIndex = Random.Range(0, totalWeights + 1);
            var taskRuneType = TaskRuneType.None;
            
            for (var i = 0; i < taskRuneWeights.Count; ++i)
            {
                if (randomIndex <= taskRuneWeights[i].Weight) // 3 < 7 ?
                {
                    taskRuneType = taskRuneWeights[i].TaskRuneType;
                    break;
                }
                
                randomIndex -= taskRuneWeights[i].Weight; // 23 - 20
            }

            if (!Verify.IsTrue(taskRuneType != TaskRuneType.None)) return null;

            TaskRune taskRune = taskRuneType switch
            {
                TaskRuneType.NoDamage    => new NoDamageTask(),
                TaskRuneType.Victory     => new VictoryTask(),
                TaskRuneType.TimeLimit   => new TimeLimitTask(300f),
                TaskRuneType.SkillCount  => new SkillCountTask(DataIndex.BloodSmash, 5),
                TaskRuneType.SkillDamage => new SkillDamageTask(DataIndex.BloodSmash, 1000f),
                _                        => null
            };

            return taskRune;
        }

        private RewardRune CreateRewardRune()
        {
            var totalWeights = rewardRuneWeights.Sum(rune => rune.Weight);
            var randomIndex = Random.Range(0, totalWeights + 1);
            var rewardRuneType = RewardRuneType.None;
            
            for (var i = 0; i < rewardRuneWeights.Count; ++i)
            {
                if (randomIndex <= rewardRuneWeights[i].Weight)
                {
                    rewardRuneType = rewardRuneWeights[i].RewardRuneType;
                    break;
                }
                
                randomIndex -= rewardRuneWeights[i].Weight;
            }
            
            if (!Verify.IsTrue(rewardRuneType != RewardRuneType.None)) return null;
            
            RewardRune rewardRune = rewardRuneType switch
            {
                RewardRuneType.Gold       => new GoldRune(500),
                RewardRuneType.Experience => new ExperienceRune(300),
                _                         => new GoldRune(-99),
            };

            rewardRune.RuneType = rewardRuneType;
            
            return rewardRune;
        }

        public EthosRune CreateRune()
        {
            var taskRune = CreateTaskRune();
            var rewardRune = CreateRewardRune();

            return new EthosRune(1, taskRune, rewardRune);
        }
        
        public IEnumerable<EthosRune> CreateRunes()
        {
            var rewardCount = Random.Range(4, 7);
            var runeList = new List<EthosRune>(rewardCount);

            for (var i = 0; i < rewardCount; i++)
            {
                var randomRewardRune = CreateRune();
                
                runeList.Add(randomRewardRune);
            }

            return runeList;
        }
    }
}
