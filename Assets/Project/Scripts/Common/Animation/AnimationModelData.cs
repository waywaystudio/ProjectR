using System;
using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;
using AnimationAsset = Spine.Unity.AnimationReferenceAsset;
using SpineAnimation = Spine.Animation;

namespace Common.Animation
{
    public class AnimationModelData : ScriptableObject
    {
        [Serializable]
        public class Index
        {
            public AnimationAsset Asset;
            public string AnimationName => Asset ? Asset.name : string.Empty;
            
            public Index(AnimationReferenceAsset asset) => Asset = asset;
        }
    
        [Serializable]
        public class TransitionIndex
        {
            public AnimationAsset From;
            public AnimationAsset Via;
            public AnimationAsset Dest;
        }

        [SerializeField] private List<Index> animationList = new();
        [SerializeField] private List<TransitionIndex> transitionIndexList = new();
        private readonly Dictionary<string, SpineAnimation> animationTable = new();
        private readonly Dictionary<(SpineAnimation, SpineAnimation), SpineAnimation> transitionTable = new();

        public Dictionary<string, SpineAnimation> AnimationTable
        {
            get
            {
                if (animationTable.IsNullOrEmpty())
                {
                    animationList.ForEach(x => animationTable.Add(x.AnimationName, x.Asset.Animation));
                }

                return animationTable;
            }
        }
    
        public Dictionary<(SpineAnimation, SpineAnimation), SpineAnimation> TransitionTable
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

        public bool TryGetAnimation(string animationName, out SpineAnimation result) 
            => AnimationTable.TryGetValue(animationName, out result);
    
        public bool TryGetTransition(SpineAnimation from, SpineAnimation dest, out SpineAnimation result) 
            => TransitionTable.TryGetValue((from, dest), out result);
    
    
        private void OnEnable()
        {
            animationList.ForEach(x => x.Asset.Initialize());
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
    
        [Sirenix.OdinInspector.Button(Sirenix.OdinInspector.ButtonSizes.Large, Icon = Sirenix.OdinInspector.SdfIconType.Download)]
        private void AssignData()
        {
            Finder.TryGetObjectList(referenceDirectory, "", out List<AnimationReferenceAsset> referenceAssetList);
        
            animationList.Clear();
            referenceAssetList.ForEach(x => animationList.Add(new Index(x)));
        }
#endif
    }
}
