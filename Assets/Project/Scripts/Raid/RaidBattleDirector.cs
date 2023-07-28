using System.Collections.Generic;
using Common;
using Common.Currencies;
using Common.Runes;
using UnityEngine;

namespace Raid
{
    public class RaidBattleDirector : MonoBehaviour
    {
        [SerializeField] private RuneCreator runeCreator;
        
        private static bool IsAnySurvivor => RaidDirector.VenturerList.Any(venturer => venturer.Alive.Value);
        public List<IReward> RewardList { get; } = new();

        public void Initialize()
        {
            AddDefeat();
            AddVictory();
            CreateReward();
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
        
        private void CreateReward()
        {
            // Gold
            RewardList.Add(GoldReward.CreateInstance(Random.Range(100, 501)));
            
            // Experience
            RewardList.Add(ExperienceReward.CreateInstance(Random.Range(10, 101)));
            
            // Runes
            RewardList.AddRange(runeCreator.CreateRunes());
        }
    }
}
