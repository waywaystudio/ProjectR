using System.Collections.Generic;
using System.Linq;
using Common.Runes.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Runes
{
    [CreateAssetMenu(menuName = "ScriptableObject", fileName = "EthosRuneCreator")]
    public class RuneCreator : ScriptableObject
    {
        [SerializeField] private List<RuneWeight> runeWeights;
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
                TaskRuneType.NoDamage    => new NoDamage(),
                TaskRuneType.Victory     => new Victory(),
                TaskRuneType.TimeLimit   => new TimeLimit(300f),
                TaskRuneType.SkillCount  => new SkillCount(DataIndex.BloodSmash, 5),
                TaskRuneType.SkillDamage => new SkillDamage(DataIndex.BloodSmash, 1000f),
                _                        => null
            };

            return taskRune;
        }

        private RewardRune CreateRewardRune()
        {
            return new RewardRune();
        }

        public EthosRune CreateRune()
        {
            var taskRune = CreateTaskRune();
            var rewardRune = CreateRewardRune();

            return new EthosRune(1, taskRune, rewardRune);
        }
    }
}
