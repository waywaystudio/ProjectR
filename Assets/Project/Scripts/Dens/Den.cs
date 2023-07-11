using System.Collections.Generic;
using Character.Villains;
using Common;
using Serialization;
using Singleton;
using UnityEngine;
// ReSharper disable CheckNamespace


public class Den : MonoSingleton<Den>, ISavable, IEditable
{
    [SerializeField] private Table<DataIndex, VillainData> villainTable = new();

    public static VillainType StageVillain { get; set; } = VillainType.LoadStonehelm;
    public static DifficultyType Difficulty { get; set; } = DifficultyType.Normal;
    public static int MythicLevel { get; set; } = 0;
    private static Table<DataIndex, VillainData> VillainTable => Instance.villainTable;

    public static VillainData GetVillainData(DataIndex dataIndex) => VillainTable[dataIndex];
    public static VillainData GetVillainData(VillainType villainIndex) => VillainTable[(DataIndex)villainIndex];
    public static bool GetVillainPrefab(VillainType type, out GameObject prefab) => Database.VillainPrefabData.GetObject((DataIndex)type, out prefab);

    public static void Spawn(VillainType villainIndex, Vector3 position, Transform hierarchy, out VillainBehaviour vb)
    {
        vb = null;
        
        if (!GetVillainPrefab(villainIndex, out var villainPrefab)) return;

        var villainObject = Instantiate(villainPrefab, position, Quaternion.identity, hierarchy);
        villainObject.SetActive(true);

        if (!villainObject.TryGetComponent(out vb)) return;

        var difficulty = Difficulty;
        var level = MythicLevel;

        vb.ForceInitialize(difficulty, level);
        vb.Initialize();
    }

    public void Save() 
    {
        villainTable.Iterate(data => data.Save());
    }

    public void Load()
    {
        villainTable.Iterate(data => data.Load());
    }
    
    
    /*
     * Reward Algorithm
     */
    public static List<GrowIngredient> GetReward(VillainType villainCode)
    {
        var result = new List<GrowIngredient>();
        var data = GetVillainData(villainCode);
        
        var ethosType = data.RepresentEthos;
        var isFirstDefeatTry = data.KillCount == 0;
        var shardOfViciousCount = 0;
        var stoneOfViciousCount = 0;
        var crystalOfPathfinderCount = 50;
        var clearProgress = Random.Range(1, 2);                    // Den에서 받기.
        var rewardMultiplier = Random.Range(1, 2) * clearProgress; // clearProgress로부터 계산.

        if (ethosType.IsVirtue())
        {
            shardOfViciousCount = 2 ;
            stoneOfViciousCount = 2 ;
        }
        else if (ethosType.IsDeficiency())
        {
            shardOfViciousCount = 3 ;
            stoneOfViciousCount = 1 ;
        }
        else if (ethosType.IsExcess())
        {
            shardOfViciousCount = 1 ;
            stoneOfViciousCount = 3 ;
        }
        
        shardOfViciousCount      *= rewardMultiplier;
        stoneOfViciousCount      *= rewardMultiplier;
        crystalOfPathfinderCount *= rewardMultiplier;

        // 경험치만.
        if (!isFirstDefeatTry)
        {
            result.Add(new GrowIngredient(GrowMaterialType.CrystalOfPathfinder, crystalOfPathfinderCount));
            return result;
        }
        
        result.Add(new GrowIngredient(GrowMaterialType.ShardOfVicious, shardOfViciousCount));
        result.Add(new GrowIngredient(GrowMaterialType.StoneOfVicious, stoneOfViciousCount));
        result.Add(new GrowIngredient(GrowMaterialType.CrystalOfPathfinder, crystalOfPathfinderCount));
        
        return result;
    }


#if UNITY_EDITOR
    public void EditorSetUp()
    {
        Finder.TryGetObjectList<VillainData>(out var villainDataList);
        
        villainTable.CreateTable(villainDataList, data => data.DataIndex);
    }
#endif
    
}

