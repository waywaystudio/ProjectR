using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Common
{
    public enum StatCode
    {
        /*00*/ None = 0,
        /*01*/ AddPower, 
        /*02*/ AddCritical,
        /*03*/ AddHaste,
        /*04*/ AddHit,
        /*05*/ AddMaxHp,
        /*06*/ AddMoveSpeed,
        /*07*/ AddArmor,
        /*08*/ AddEvade,
        /*09*/ AddResist,
        /*101*/ MultiPower = 101,
        /*102*/ MultiCritical, 
        /*103*/ MultiHaste,
        /*104*/ MultiHit,
        /*105*/ MultiMaxHp,
        /*106*/ MultiMoveSpeed,
        /*107*/ MultiArmor,
        /*108*/ MultiEvade,
        /*109*/ MultiResist,
    }
    
    [Serializable]
    public class StatTable : Dictionary<StatCode, FloatTable>
    {
        public float Power => Get(StatCode.AddPower) * (1f + Get(StatCode.MultiPower));
        public float Critical => Get(StatCode.AddCritical) * (1f + Get(StatCode.MultiCritical));
        public float Haste => Get(StatCode.AddHaste) * (1f + Get(StatCode.MultiHaste));
        public float Hit => Get(StatCode.AddHit) * (1f + Get(StatCode.MultiHit));
        public float MaxHp => Get(StatCode.AddMaxHp) * (1f + Get(StatCode.MultiMaxHp));
        public float MoveSpeed => Get(StatCode.AddMoveSpeed) * (1f + Get(StatCode.MultiMoveSpeed));
        public float Armor => Get(StatCode.AddArmor) * (1f + Get(StatCode.MultiArmor));
        public float Evade => Get(StatCode.AddEvade) * (1f + Get(StatCode.MultiEvade));
        public float Resist => Get(StatCode.AddResist) * (1f + Get(StatCode.MultiResist));

        public void Register(StatCode statCode, int partKey, Func<float> function, bool overwrite)
        {
            if (ContainsKey(statCode))
            {
                this[statCode].Register(partKey, function, overwrite);
            }
        }
        public void Register(StatCode statCode, FloatTable table, bool overwrite)
        {
            if (ContainsKey(statCode) && overwrite && this[statCode].Result < table.Result) this[statCode] = table;
            else TryAdd(statCode, table);
        }
        
        public void Unregister(StatCode statCode, int partKey)
        {
            if (ContainsKey(statCode)) this[statCode].Unregister(partKey);
        }
        
        public void Unregister(StatCode statCode) => this.TryRemove(statCode);
        private float Get(StatCode statCode) => TryGetValue(statCode, out var floatTable) ? floatTable.Result : 0f;
    }

    [Serializable]
    public class FloatTable : Dictionary<int, Func<float>>
    {
        public float Result
        {
            get
            {
                var result = 0f;
                this.ForEach(x => result += x.Value.Invoke());
                return result;
            }
        }

        public void Register(int key, Func<float> function, bool overwrite)
        {
            if (ContainsKey(key) && 
                overwrite && 
                Abs(this[key].Invoke()) < Abs(function.Invoke())) this[key] = function;
            else 
                TryAdd(key, function);
        }

        public void Unregister(int key) => this.TryRemove(key);
        private static float Abs(float value) => value < 0 ? value * -1.0f : value;
    }
    
    
    // public static class StatCode
    // {
    //     private const string AddPowerCode = "AddPower";
    //     private const string AddCriticalCode = "AddCritical";
    //     private const string AddHasteCode = "AddHaste";
    //     private const string AddHitCode = "AddHit";
    //     private const string AddMaxHpCode = "AddMaxHp";
    //     private const string AddMoveSpeedCode = "AddMoveSpeed";
    //     private const string AddArmorCode = "AddArmor";
    //     private const string AddEvadeCode = "AddEvade";
    //     private const string AddResistCode = "AddResist";
    //
    //     private const string MultiPowerAddCode = "MultiPower";
    //     private const string MultiCriticalCode = "MultiCritical";
    //     private const string MultiHasteCode = "MultiHaste";
    //     private const string MultiHitCode = "MultiHit";
    //     private const string MultiMaxHpCode = "MultiMaxHp";
    //     private const string MultiMoveSpeedCode = "MultiMoveSpeed";
    //     private const string MultiArmorCode = "MultiArmor";
    //     private const string MultiEvadeCode = "MultiEvade";
    //     private const string MultiResistCode = "MultiResist";
    //
    //     [ShowInInspector] public static readonly int AddPower = AddPowerCode.GetHashCode();
    //     [ShowInInspector] public static readonly int AddCritical = AddCriticalCode.GetHashCode();
    //     [ShowInInspector] public static readonly int AddHaste = AddHasteCode.GetHashCode();
    //     [ShowInInspector] public static readonly int AddHit = AddHitCode.GetHashCode();
    //     [ShowInInspector] public static readonly int AddMaxHp = AddMaxHpCode.GetHashCode();
    //     [ShowInInspector] public static readonly int AddMoveSpeed = AddMoveSpeedCode.GetHashCode();
    //     [ShowInInspector] public static readonly int AddArmor = AddArmorCode.GetHashCode();
    //     [ShowInInspector] public static readonly int AddEvade = AddEvadeCode.GetHashCode();
    //     [ShowInInspector] public static readonly int AddResist = AddResistCode.GetHashCode();
    //
    //     [ShowInInspector] public static readonly int MultiPower = MultiPowerAddCode.GetHashCode();
    //     [ShowInInspector] public static readonly int MultiCritical = MultiCriticalCode.GetHashCode();
    //     [ShowInInspector] public static readonly int MultiHaste = MultiHasteCode.GetHashCode();
    //     [ShowInInspector] public static readonly int MultiHit = MultiHitCode.GetHashCode();
    //     [ShowInInspector] public static readonly int MultiMaxHp = MultiMaxHpCode.GetHashCode();
    //     [ShowInInspector] public static readonly int MultiMoveSpeed = MultiMoveSpeedCode.GetHashCode();
    //     [ShowInInspector] public static readonly int MultiArmor = MultiArmorCode.GetHashCode();
    //     [ShowInInspector] public static readonly int MultiEvade = MultiEvadeCode.GetHashCode();
    //     [ShowInInspector] public static readonly int MultiResist = MultiResistCode.GetHashCode();
    // }
    
}
