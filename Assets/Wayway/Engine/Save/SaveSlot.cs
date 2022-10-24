using System;
using UnityEngine;

namespace Wayway.Engine.Save
{
    public class SaveSlot : MonoBehaviour
    {
        [SerializeField] private string saveName;
        
        private DateTime lastSaveTime;
        private ES3File es3File;
    }
}
