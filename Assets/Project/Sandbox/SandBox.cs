using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class SandBox : MonoBehaviour
{
    [FolderPath]
    public string SpritePath;

    public string target1SpriteName;
    public string target2SpriteName;
    
    public Sprite Target1;
    public Sprite Target2;
    public void ShowDebugMessage() => Debug.Log("Is In!");

    [Button]
    public void Debugger()
    {
        Finder.TryGetSprite(SpritePath, target1SpriteName, out Target1);
        Finder.TryGetSprite(SpritePath, target2SpriteName, out Target2);
    }
    
    [Button]
    public void ResetSprite()
    {
        Target1 = null;
        Target2 = null;
    }
}
