using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Common.Character.Graphic
{
    public class AnimationEventModel : MonoBehaviour
    {
        /*
         * Animation Event Key
         * attackHit
         * footstep
         * skillHit
         * + add more Event Key in Spine program first...
         */
        [SerializeField] private SkeletonAnimation skAnimation;
        [SerializeField] private CharacterBehaviour cb;

        private SkeletonAnimation SkAnimation => skAnimation ??= GetComponent<SkeletonAnimation>();
        private CharacterBehaviour Cb => cb ??= GetComponentInParent<CharacterBehaviour>();
        

        private void OnEnable()
        {
            SkAnimation.AnimationState.Event += EventHandler;
        }

        private void Start()
        {
            SkAnimation.Initialize(false);
        }

        private void EventHandler(TrackEntry trackEntry, Event e)
        {
            switch (e.Data.Name)
            {
                case "attackHit" : Cb.SkillHit(); break;
                case "skillHit" : Cb.SkillHit(); break;
                case "channelingHit" : Cb.SkillHit(); break;
            }
        }

        private void OnDisable()
        {
            SkAnimation.AnimationState.Event -= EventHandler;
        }

#if UNITY_EDITOR
        [SpineEvent(dataField : "skAnimation", fallbackToTextField = true)]
        [SerializeField] private string eventNameList;
#endif
    }
}