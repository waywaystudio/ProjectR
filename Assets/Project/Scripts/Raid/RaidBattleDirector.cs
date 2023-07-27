using System.Collections.Generic;
using System.Linq;
using Common.Runes;
using UnityEngine;

namespace Raid
{
    public class RaidBattleDirector : MonoBehaviour
    {
        [SerializeField] private RuneCreator runeCreator;
        
        private static bool IsAnySurvivor => RaidDirector.VenturerList.Any(venturer => venturer.Alive.Value);
        
        public void Initialize()
        {
            AddDefeat();
            AddVictory();
            
            // Add Reward
            // Victory Effect -> Get Reward
        }

        public void OnRaidWin()
        {
            AddRuneReward();
        }

        public void OnRaidDefeat()
        {
            // AddRuneReward();
        }


        private static void AddVictory()
        {
            var villain = RaidDirector.Villain;
            var victoryEvent = RaidDirector.OnRaidWin;
            var builder = new SequenceBuilder(villain.DeadBehaviour.Sequence);

            builder
                .Add(Section.Active, "CallVictoryForVenturer", victoryEvent.Invoke)
                ;
        }

        private static void AddDefeat()
        {
            var venturers = RaidDirector.VenturerList;
            var defeatEvent = RaidDirector.OnRaidDefeat;
            
            venturers.ForEach(venturer =>
            {
                var builder = venturer.DeadBehaviour.Builder;

                builder
                    .Add(Section.Active, "CheckDefeat", () => { if (!IsAnySurvivor) defeatEvent.Invoke(); })
                    ;
            });
        }

        // TODO. Reward를 할당하는 Class위치가 이 곳이 아닐 것 같다.
        private void AddRuneReward()
        {
            var villain = RaidDirector.Villain;
            var rewardCount = Random.Range(4, 7);
            var runeList = new List<EthosRune>(rewardCount);

            for (var i = 0; i < rewardCount; i++)
            {
                var randomRewardRune = runeCreator.CreateRune();
                
                runeList.Add(randomRewardRune);
            }
            
            Den.GetVillainData(villain.DataIndex).KillCount++;
            Camp.AddRunes(runeList);

            // TODO.TEMP
            // runeList.ForEach(rune => Debug.Log($"Get Rune:{rune.TaskDescription}"));
        }
    }
}
