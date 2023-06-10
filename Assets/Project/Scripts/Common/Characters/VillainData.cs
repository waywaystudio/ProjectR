using Serialization;
using UnityEngine;

namespace Common.Characters
{
    public class VillainData : CharacterData
    {
        [SerializeField] private string fullName;
        [SerializeField] private string subName;
        [SerializeField] private string description;
        [SerializeField] private EthosType representEthos;
        [SerializeField] private int killCount;
        
        public VillainType VillainType => (VillainType)characterIndex;
        public DifficultyType Difficulty { get; set; } = DifficultyType.Normal;
        public EthosType RepresentEthos => representEthos;
        public string FullName => fullName;
        public string SubName => subName;
        public string Description => description;
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

            var data = Database.BossSheetData(DataIndex);

            fullName    = data.FullName;
            subName     = data.SubName;
            description = data.BackgroundNarrative;
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
#endif
    }
}
