using System;
using Core;
using UnityEngine;

namespace Character
{
    /* Status에 각 값을 Observable<T>로 처리하고 싶었으나, Clamp 때문에 불가능...
     추후에 아이디어가 떠오르면 고쳐보자. */
    
    [Serializable]
    public class Status
    {
        public Status() : this(null) {}
        public Status(StatTable statTable)
        {
            StatTable = statTable;
            isAlive = true;
        }

        [SerializeField] private bool isAlive;
        [SerializeField] private float hp;
        [SerializeField] private float resource;
        [SerializeField] private float shield;

        public ActionTable<bool> OnAliveChanged { get; } = new();
        public ActionTable<float> OnHpChanged { get; } = new();
        public ActionTable<float> OnResourceChanged { get; } = new();
        public ActionTable<float> OnShieldChanged { get; } = new();
        public StatTable StatTable { get; set; }

        public bool IsAlive
        {
            get => isAlive;
            set
            {
                isAlive = value;
                OnAliveChanged?.Invoke(value);
            }
        }
        
        public float Hp
        {
            get => hp;
            set
            {
                hp = Mathf.Min(StatTable.MaxHp, value);
                OnHpChanged?.Invoke(value);
            }
        }
        
        public float Resource
        {
            get => resource;
            set
            {
                resource = Mathf.Min(StatTable.MaxResource, value);
                OnResourceChanged?.Invoke(value);
            }
        }

        public float Shield
        {
            get => shield;
            set
            {
                // Shield can stacked only (maxHp / 10)
                shield = Mathf.Min(StatTable.MaxHp * 0.1f, value);
                OnShieldChanged?.Invoke(value);
            }
        }
    }
}
