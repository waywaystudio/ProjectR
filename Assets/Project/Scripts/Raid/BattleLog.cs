using System.Collections.Generic;
using System.Threading;
using Character.Venturers;
using Character.Villains;
using Common;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Raid
{
    public class BattleLog
    {
        public List<VenturerBehaviour> PartyList;
        public VillainBehaviour VillainBehaviour;

        private CancellationTokenSource cts;
        private float timer;
        private Dictionary<ICombatProvider, List<CombatEntity>> LogTable { get; } = new();

        // venturer key // skill key // combatEntity;
        
        // by Venturer
        /// by Skill?
        // 무슨 스킬 몇 뎀, 몇 번, 
        public float Damage { get; set; }
        public float Heal { get; set; }
        public bool IsCritical { get; set; }
        
        // venturer min Hp.
        // venturer moveDistance
        public void Initialize()
        {
            PartyList        = RaidDirector.VenturerList;
            VillainBehaviour = RaidDirector.Villain;
            
            PartyList.ForEach(venturer =>
            {
                venturer.OnCombatProvided.Add("BattleLog", AddBattleLog);
                // venturer.EthosRune.Initialize();
            });
            VillainBehaviour.OnCombatProvided.Add("BattleLog", AddBattleLog);
            
            PlayTimer().Forget();
        }

        public void Dispose()
        {
            PartyList.ForEach(venturer =>
            {
                venturer.OnCombatProvided.Remove("BattleLog");
                // venturer.EthosRune.Accomplish(/*param*/);
            });
            VillainBehaviour.OnCombatProvided.Remove("BattleLog");
            
            StopTimer();
        }

        public void AddBattleLog(CombatEntity entity)
        {
            if (!LogTable.ContainsKey(entity.Provider))
                LogTable.Add(entity.Provider, new List<CombatEntity>());

            LogTable[entity.Provider].AddUniquely(entity);
        }


        private async UniTaskVoid PlayTimer()
        {
            timer = 0f;
            cts   = new CancellationTokenSource();

            while (!cts.Token.IsCancellationRequested)
            {
                timer += Time.deltaTime;

                await UniTask.Yield(cts.Token);
            }
        }

        private void StopTimer()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
