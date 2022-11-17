using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;

public class AnimationModelData : ScriptableObject
{
    [Serializable]
    public class AnimationIndex
    {
        public AnimationIndex(AnimationReferenceAsset asset)
        {
            AnimationHash = Animator.StringToHash(asset.name);
            AnimationReferenceAsset = asset;
        }
        
        public int AnimationHash;
        public AnimationReferenceAsset AnimationReferenceAsset;
        
        public string AnimationName => AnimationReferenceAsset ? AnimationReferenceAsset.name 
                                                               : string.Empty;
    }
    
    [Serializable]
    public class AnimationTransitionIndex
    {
        public AnimationReferenceAsset From;
        public AnimationReferenceAsset Via;
        public AnimationReferenceAsset Dest;
    }

    [SerializeField] private List<AnimationIndex> animationList = new();
    [SerializeField] private List<AnimationTransitionIndex> transitionIndexList = new();

    private void OnEnable()
    {
        animationList.ForEach(x => x.AnimationReferenceAsset.Initialize());
        transitionIndexList.ForEach(x =>
        {
            x.From.Initialize();
            x.Via.Initialize();
            x.Dest.Initialize();
        });
    }

    public bool TryGetAnimation(int nameHash, out Animation result)
    {
        result = animationList.Find(x => x.AnimationHash == nameHash)
                              .AnimationReferenceAsset.Animation;

        return result is not null;
    }
    
    public bool TryGetTransition(Animation from, Animation dest, out Animation result)
    {
        result 
            = transitionIndexList.Find(x => x.From.Animation == from && x.Dest.Animation == dest)?
                .Via.Animation;

        return result is not null;
    }

#if UNITY_EDITOR
    [SerializeField, FolderPath] private string referenceDirectory;
    [Button]
    private void AssignData()
    {
        Finder.TryGetObjectList(referenceDirectory, "", out List<AnimationReferenceAsset> referenceAssetList);
        
        animationList.Clear();
        referenceAssetList.ForEach(x => animationList.Add(new AnimationIndex(x)));
    }
#endif
}
