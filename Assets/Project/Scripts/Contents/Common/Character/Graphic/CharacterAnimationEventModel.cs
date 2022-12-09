using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEngine;
using Event = Spine.Event;

namespace Common.Character
{
    public class CharacterAnimationEventModel : MonoBehaviour
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
        private Dictionary<string, EventData> EventTable { get; set; } = new();

        private void Start()
        {
            SkAnimation.Initialize(false);
            
            var eventHolder = SkAnimation.Skeleton.Data;
            eventHolder.Events.ForEach(x => EventTable.Add(x.Name, eventHolder.FindEvent(x.Name)));
            
            SkAnimation.AnimationState.Event += EventHandler;
        }

        private void EventHandler(TrackEntry trackEntry, Event e)
        {
            if (e.Data == EventTable["attackHit"])
            {
                Cb.AttackHit();
            }
            else if (e.Data == EventTable["footstep"])
            {
                Cb.Footstep();
            }
            else if (e.Data == EventTable["skillHit"])
            {
                Cb.SkillHit();
            }
        }

        private void OnDisable()
        {
            SkAnimation.AnimationState.Event -= EventHandler;
        }

#if UNITY_EDITOR
        [SpineEvent(dataField : "skAnimation", fallbackToTextField = true)]
        [ShowInInspector] private string eventNameList;
#endif
    }
}