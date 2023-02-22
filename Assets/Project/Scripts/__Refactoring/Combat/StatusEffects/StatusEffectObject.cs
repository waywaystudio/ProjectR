// using System.Collections;
// using Core;
// using UnityEngine;
// using UnityEngine.Pool;
//
// namespace Character.Combat.StatusEffects
// {
//     public abstract class StatusEffectObject : CombatObject, IStatusEffect
//     {
//         [SerializeField] protected Sprite icon;
//         [SerializeField] protected bool isBuff;
//         [SerializeField] protected float duration;
//         [SerializeField] protected float combatValue;
//
//         protected ICombatTaker Taker;
//         protected Coroutine RoutineBuffer;
//         protected WaitForSeconds WaitBuffer;
//         private IObjectPool<StatusEffectObject> pool;
//
//         public Sprite Icon => icon;
//         public bool IsBuff => isBuff;
//         public virtual float Duration => duration;
//         public float CombatValue => combatValue;
//         public StatusEffectTable TargetTable { get; set; }
//
//
//         public virtual void Initialize(ICombatProvider provider, ICombatTaker taker)
//         {
//             Provider = provider;
//             Taker    = taker;
//
//             Active();
//         }
//
//         public void SetPool(IObjectPool<StatusEffectObject> pool) => this.pool = pool;
//
//         public override void Active()
//         {
//             base.Active();
//             
//             Taker.TakeStatusEffect(this);
//             RoutineBuffer = StartCoroutine(Effectuate());
//         }
//
//         public override void Complete()
//         {
//             base.Complete();
//             
//             UnregisterTable();
//
//             RoutineBuffer = null;
//             pool.Release(this);
//         }
//
//         
//         protected abstract IEnumerator Effectuate();
//
//         private void UnregisterTable()
//         {
//             if (TargetTable.HasElement())
//             {
//                 TargetTable.Unregister(this);
//                 TargetTable = null;
//             }
//         }
//         
//
//
// #if UNITY_EDITOR
//         public override void SetUp()
//         {
//             base.SetUp();
//             
//             var data = MainGame.MainData.GetStatusEffect(actionCode);
//             
//             // GetComponents<OldCombatModule>().ForEach(x => ModuleUtility.SetStatusEffectModule(data, x));
//
//             // icon     = data.~~~
//             isBuff      = data.IsBuff;
//             duration    = data.Duration;
//             combatValue = data.CombatValue;
//         }
// #endif
//     }
// }
