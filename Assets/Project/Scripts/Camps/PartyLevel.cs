using System;
using GameEvents;
using Serialization;
using UnityEngine;

namespace Camps
{
    [Serializable]
    public class PartyLevel
    {
        [SerializeField] private int level;
        [SerializeField] private int experience;
        [SerializeField] private GameEventInt OnExperienceChanged;
        [SerializeField] private GameEventInt OnLevelChanged;

        private const string SerializeKey = "Camp.PartyLevel";

        public int Level => level;

        public int Experience
        {
            get => experience;
            set
            {
                if (experience == value) return;

                experience = value;
                OnExperienceChanged.Invoke(value);
            }
        }

        public void Save()
        {
            Serializer.Save($"{SerializeKey}.Level", level);
            Serializer.Save($"{SerializeKey}.Experience", experience);
        }

        public void Load()
        {
            level      = Serializer.Load($"{SerializeKey}.Level", 1);
            experience = Serializer.Load($"{SerializeKey}.Experience", 0);
        }
    }
}
