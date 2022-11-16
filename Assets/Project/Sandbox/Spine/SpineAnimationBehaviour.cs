using System;
using Spine.Unity;
using UnityEngine;

public class SpineAnimationBehaviour : StateMachineBehaviour
{
    public AnimationClip Motion;
    public float TimeScale = 1.0f;
    public int layer = 0;
    
    private string animationClip;
    private bool loop;
    private SkeletonAnimation skeletonAnimation;
    private Spine.AnimationState spineAnimationState;
    private Spine.TrackEntry trackEntry;

    private void Awake()
    {
        if (!Motion) return;
        
        animationClip = Motion.name;
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!skeletonAnimation)
        {
            skeletonAnimation = animator.GetComponentInChildren<SkeletonAnimation>();
            spineAnimationState = skeletonAnimation.state;
        }

        loop = stateInfo.loop;
        trackEntry = spineAnimationState.SetAnimation(layer, animationClip, loop);
        trackEntry.TimeScale = TimeScale;
    }
}
