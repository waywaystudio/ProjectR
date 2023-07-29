using System.Collections.Generic;
using Common;
using Common.Runes;
using UnityEngine;

namespace Character.Venturers
{
    public class VenturerEthosRunes : MonoBehaviour
    {
        public RewardCreator RewardCreator;
        
        [SerializeField] private List<EthosRune> ethosRuneList;
        
        public List<EthosRune> EthosRuneList => ethosRuneList;

        public void Initialize(VenturerBehaviour vb)
        {
            // TODO TEMP - 추후에는 ActiveTask만.
            var randomRune = RewardCreator.CreateRune();
            
            ethosRuneList.Clear();
            ethosRuneList.Add(randomRune);
            ethosRuneList.ForEach(rune =>
            {
                rune.Assign(vb);
                rune.ActiveTask();
            });
        }
    }
}
