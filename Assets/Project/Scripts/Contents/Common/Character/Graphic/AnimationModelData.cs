using System;
using System.Collections.Generic;
using Core;
using Spine.Unity;
using UnityEngine;
using Animation = Spine.Animation;

namespace Common.Character.Graphic
{
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
        private readonly Dictionary<string, Animation> animationTable = new();
        private readonly Dictionary<(Animation, Animation), Animation> transitionTable = new();

        public Dictionary<string, Animation> AnimationTable
        {
            get
            {
                if (animationTable.IsNullOrEmpty())
                {
                    animationList.ForEach(x => animationTable.Add(x.AnimationName, x.AnimationReferenceAsset.Animation));
                }

                return animationTable;
            }
        }
    
        public Dictionary<(Animation, Animation), Animation> TransitionTable
        {
            get
            {
                if (transitionTable.IsNullOrEmpty())
                {
                    transitionIndexList.ForEach(x => 
                        transitionTable.Add((x.From, x.Dest), x.Via));
                }

                return transitionTable;
            }
        }

        public bool TryGetAnimation(string animationName, out Animation result) 
            => AnimationTable.TryGetValue(animationName, out result);
    
        public bool TryGetTransition(Animation from, Animation dest, out Animation result) 
            => TransitionTable.TryGetValue((from, dest), out result);
    
    
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

#if UNITY_EDITOR
        [SerializeField, Sirenix.OdinInspector.FolderPath] 
        private string referenceDirectory;
    
        [Sirenix.OdinInspector.Button]
        private void AssignData()
        {
            Finder.TryGetObjectList(referenceDirectory, "", out List<AnimationReferenceAsset> referenceAssetList);
        
            animationList.Clear();
            referenceAssetList.ForEach(x => animationList.Add(new AnimationIndex(x)));
        }
#endif
    }
}
