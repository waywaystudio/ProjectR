using System;
using GameEvents;
using Serialization;
using UnityEngine;

namespace Camps.Storages
{
    [Serializable]
    public class GoldStorage
    {
        [SerializeField] private int gold;
        [SerializeField] private GameEventInt onGoldChanged;

        private const string SerializeKey = "Camp.Gold";

        public int Gold
        {
            get => gold;
            set
            {
                if (gold == value) return;
                
                onGoldChanged.Invoke(value);
                gold = value;
            }
        }

        public void Save()
        {
            Serializer.Save(SerializeKey, gold);
        }

        public void Load()
        {
            gold = Serializer.Load(SerializeKey, 0);
        }
    }
}
