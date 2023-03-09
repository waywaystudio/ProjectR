using Core;
using UnityEngine;

namespace Raid.UI
{
    using BossFrames;
    
    public class BossFrame : MonoBehaviour, IEditable
    {
        [SerializeField] private BossHpProgress hpProgress;

        public void Initialize()
        {
            hpProgress.Initialize();
        }
        

#if UNITY_EDITOR
        public void EditorSetUp()
        {
            hpProgress ??= GetComponentInChildren<BossHpProgress>();
        }
#endif
    }
}
