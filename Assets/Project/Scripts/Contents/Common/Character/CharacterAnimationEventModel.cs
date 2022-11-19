using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Events;
using Event = Spine.Event;

namespace Common.Character
{
    public class CharacterAnimationEventModel : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        
        [SerializeField, SpineEvent(dataField: "skeletonAnimation", fallbackToTextField: true)]
        private string eventName;
        [SerializeField] private UnityEvent animationEvent;
        private EventData eventData;
    
        private void Start()
        {
            skeletonAnimation.Initialize(false);
            eventData = skeletonAnimation.Skeleton.Data.FindEvent(eventName);
    
            skeletonAnimation.AnimationState.Event += Invoke;
        }
    
        private void Invoke(TrackEntry trackEntry, Event e)
        {
            if (eventData == e.Data)
            {
                animationEvent?.Invoke();
                EventTester();
            }
        }
    
        private void EventTester() => Debug.Log($"{eventName} In!");
    }
}
