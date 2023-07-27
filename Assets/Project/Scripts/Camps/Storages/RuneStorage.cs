using System;
using System.Collections.Generic;
using Common.Runes;
using Serialization;

namespace Camps.Storages
{
    [Serializable]
    public class RuneStorage
    {
        public const string SerializeKey = "RuneStorage";
        
        [Sirenix.OdinInspector.ShowInInspector]
        public List<EthosRune> RuneList { get; } = new();

        public void AddRune(EthosRune rune) => RuneList.Add(rune);
        public void AddRunes(IEnumerable<EthosRune> runeList) => RuneList.AddRange(runeList);
        public void RemoveRune(EthosRune rune) => RuneList.Remove(rune);


        public void Save()
        {
            Serializer.Save(SerializeKey, RuneList);
        }

        public void Load()
        {
            RuneList.Clear();
            RuneList.AddRange(Serializer.Load(SerializeKey, new List<EthosRune>()));
        }
    }
}
