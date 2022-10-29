using System;
using UnityEngine;

namespace Wayway.Engine.Save
{
    // TODO.
    /* SaveInfo 클래스의 Save(), Load(), Delete()는 사실 여기 없어도 된다.
     Save UI에서 위의 함수들을 이전시키자.
     SaveInfo는 데이타 클래스로 만들자. 
     -> SaveInfo에 SaveManager 디펜던시를 없에자. */

    [Serializable]
    public class SaveInfo : ISavable
    {
        public SaveInfo(string saveName)
        {
            this.saveName = saveName;
            saveTime = DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss");
            lastSceneName = "Lobby";
            
            // TODO.
            // Link project version later...
            version = 0.1f;
        }

        [SerializeField] private string saveName;
        [SerializeField] private string saveTime;
        [SerializeField] private string lastSceneName;
        [SerializeField] private float version;

        public string SaveName => saveName;
        public string SaveTime => saveTime;
        public string LastSceneName => lastSceneName;
        public float Version => version;
        
        public void Save()
        {
            saveTime = DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss");
            SaveManager.SaveToSlot(this);
        }

        public void Load()
        {
            SaveManager.LoadFromSlot(this);
        }

        public void Delete()
        {
            SaveManager.DeleteSlot(this);
        }
    }
}
