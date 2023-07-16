using Raid.UI.BossFrames;
using Raid.UI.FloatingTexts;
using Raid.UI.VenturerFrames;
using UnityEngine;
using UnityEngine.Serialization;

namespace Raid
{
    public class RaidUIDirector : MonoBehaviour, IEditable
    {
        // TODO. 추후에 bossFrame 한꺼번에 Initialize하기
        [SerializeField] private BossHpProgress bossHpProgress;
        
        // Venturer Frames
        [SerializeField] private VenturerSkillBar skillBar;

        // Pool
        [FormerlySerializedAs("floatingTextPool")] [SerializeField] private CombatTextPool combatTextPool;


        public void Initialize()
        {
            bossHpProgress.Initialize();
            skillBar.Initialize();
        }


#if UNITY_EDITOR
        public void EditorSetUp()
        {
            skillBar         ??= GetComponentInChildren<VenturerSkillBar>();
            combatTextPool ??= GetComponentInChildren<CombatTextPool>();
        }
#endif
    }
}
