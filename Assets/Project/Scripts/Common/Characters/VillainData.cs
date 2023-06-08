using Serialization;
using UnityEngine;

namespace Common.Characters
{
    public class VillainData : CharacterData
    {
        [SerializeField] private EthosType representEthos;
        [SerializeField] private int killCount;
        
        public DifficultyType Difficulty { get; set; } = DifficultyType.Normal;
        public EthosType RepresentEthos => representEthos;
        public int KillCount => killCount;

        public override void Save()
        {
            base.Save();
            
            SaveManager.Save($"{characterIndex.ToString()}.KillCount", killCount);
        }
        public override void Load()
        {
            base.Load();
            
            killCount = SaveManager.Load($"{characterIndex.ToString()}.KillCount",0);
        }


#if UNITY_EDITOR
        public override void EditorSetUp()
        {
            base.EditorSetUp();

            mask           = CharacterMask.Villain;
            representEthos = characterIndex.ConvertToEthosType();
        }
#endif
    }
}
