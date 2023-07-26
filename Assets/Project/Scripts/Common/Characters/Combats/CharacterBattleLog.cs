using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Common.Characters.Combats
{
    public class CharacterBattleLog : MonoBehaviour
    {
        private CancellationTokenSource cts;

        public float Timer { get; private set; } = 1f;
        public ProviderLog ProvideLog { get; } = new();
        public TakerLog TakeLog { get; } = new();

        #region PRESET
        public int HitCount => ProvideLog.HitCount;
        public int CriticalHitCount => ProvideLog.CriticalHitCount;
        public int BeatCount => TakeLog.BeatCount;
        public int BeCriticalHitCount => TakeLog.BeCriticalHitCount;
        public float TotalDamage => ProvideLog.TotalDamage;
        public float TotalHeal => ProvideLog.TotalHeal;
        public float TotalDamaged => TakeLog.TotalDamaged;
        public float TotalHealed => TakeLog.TotalHealed;
        public float Dps => TotalDamage / Timer;
        public float Hps => TotalHeal / Timer;
        public float Tps => TotalDamaged / Timer;
        public float Thps => TotalHealed / Timer;
        // public float ValueOf(DataIndex combatIndex) => ProvideLog.GetSkillTotalValues(combatIndex);
        #endregion

        public void Initialize(CharacterBehaviour cb)
        {
            ProvideLog.Initialize(cb.OnCombatProvided);
            TakeLog.Initialize(cb.OnCombatTaken);
            
            StartRecord().Forget();
        }

        public void Dispose()
        {
            ProvideLog.Dispose();
            TakeLog.Dispose();
            
            StopRecord();
        }


        private async UniTaskVoid StartRecord()
        {
            Timer = 0f;
            cts   = new CancellationTokenSource();

            while (!cts.Token.IsCancellationRequested)
            {
                Timer += Time.deltaTime;

                await UniTask.Yield(cts.Token);
            }
        }

        private void StopRecord()
        {
            cts?.Cancel();
            cts = null;
        }
    }
}
