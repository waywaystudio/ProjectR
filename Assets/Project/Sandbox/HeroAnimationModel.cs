using Core;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;

public class HeroAnimationModel : MonoBehaviour
{
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    [SerializeField] private AnimationModelData modelData;
    [SerializeField, SpineAnimation(dataField: "skeletonAnimation.skeletonDataAsset")]
    private string startAnimation;
    
    public Animation TargetAnimation { get; private set; }
    
    // Look Where;
    // Do What;
    
    // Test
    [Button] public void Idle() => Play("idle", 0);
    [Button] public void Attack() => Play("attack", 0);
    [Button] public void Walk() => Play("walk", 0);
    [Button] public void Run() => Play("run", 0);
    [Button] public void Crouch() => Play("crouch", 0);
    
    private void Awake()
    {
        skeletonAnimation ??= GetComponentInChildren<SkeletonAnimation>();
        skeletonAnimation.AnimationState
            .SetAnimation(0, startAnimation.IsNullOrEmpty() ? "idle" : startAnimation, true);
    }
    
    public void SetDirection(Vector2 input)
    {
        skeletonAnimation.Skeleton.ScaleX = input.x switch
        {
            < 0 => -1.0f,
            > 0 => 1.0f,
            _ => skeletonAnimation.Skeleton.ScaleX
        };
    }
    
    public void Play(string animationKey, int layer) => Play(Animator.StringToHash(animationKey), layer);
    public void Play(int nameHash, int layer)
    {
        if (!modelData.TryGetAnimation(nameHash, out var target)) return;
    
        Play(target, layer);
    }
    public void Play(Animation target, int layer)
    {
        var hasCurrent = TryGetCurrentAnimation(layer, out var current);
        var hasTransition = modelData.TryGetTransition(current, target, out var transition);
        
        if (hasCurrent && hasTransition)
        {
            skeletonAnimation.AnimationState.SetAnimation(layer, transition, false);
            skeletonAnimation.AnimationState.AddAnimation(layer, target, true, 0f);
    
            return;
        }
        
        skeletonAnimation.AnimationState.SetAnimation(layer, target, true);
        TargetAnimation = target;
    }
    
    public void PlayOneShot(Animation oneShot, int layer)
    {
        var state = skeletonAnimation.AnimationState;
    
        state.SetAnimation(0, oneShot, false);
    
        if (modelData.TryGetTransition(oneShot, TargetAnimation, out var transition))
        {
            state.AddAnimation(0, transition, false, 0f);
        }
    
        state.AddAnimation(0, TargetAnimation, true, 0f);
    }
    
    public bool TryGetCurrentAnimation(int layer, out Animation result)
    {
        result = skeletonAnimation.AnimationState.GetCurrent(layer)?.Animation;
    
        return result is not null;
    }
}
