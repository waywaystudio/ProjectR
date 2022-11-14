using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlotComponent : MonoBehaviour
{
    // 세이브 슬롯이라고 생각해보자.
    // 로드 목적으로 불려짐 (시작 시, 로드메뉴를 통함)
    // 세이브 목적으로 불려짐 (빈 슬롯에 세이브, 기존 슬롯 오버라이드)
    // 빈 슬롯.
    // - 빈 슬롯에 세이브
    // 슬롯에 세이브 되어 있는 SaveInfo
    // - 슬롯정보 보이기
    // - 로드

    private bool isSavable;

    public bool IsLoadable => !isSavable;
    public bool IsSavable => isSavable;
    
    public void CreateNewSlot()
    {
        
    }

    public void LoadFromSlot()
    {
            
    }

    public void SaveToSlot()
    {
            
    }

    public void SaveToOverride()
    {
            
    }

    public void DeleteSlot()
    {
            
    }
    
    // public void CreateNewSlot(string saveFileName)
    // {
    //     CreateNewSaveFile(saveFileName);
    //     RefreshSaveInfoList();
    // }
    //
    // public void LoadFromSlot(SaveInfo saveInfo)
    // {
    //     CopySaveFile(saveInfo.SaveName, PlaySaveFile);
    //         
    //     // TODO.
    //     // Scene Change Action in here?
    // }
    //
    // public void SaveToSlot(SaveInfo saveInfo)
    // {
    //     PlaySave();
    //     CopySaveFile(PlaySaveFile, saveInfo.SaveName);
    // }
    //
    // public void DeleteSlot(SaveInfo saveInfo)
    // {
    //     DeleteSaveFile(saveInfo.SaveName);
    //     RefreshSaveInfoList();
    // }
    
}
