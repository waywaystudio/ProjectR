using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEngine;

namespace Common.Animation
{
    public class AnimationUISkinController : MonoBehaviour
    {
        [SerializeField] private SkeletonGraphic skeletonAnimation;
        
        private SkinEntity skinEntity;

        public void Initialize(SkinEntity skinEntity)
        {
            this.skinEntity = skinEntity;

            ApplySkinChanges();
        }
        

        [Button(ButtonSizes.Large)]
        public void ApplySkinChanges()
        {
            if (skeletonAnimation == null)
            {
                Debug.LogWarning("Not exist skeletonAnimation");
                return;
            }

            //Gets the skeleton and its data from the character gameObject
            var skeleton = skeletonAnimation.Skeleton;
            var skeletonData = skeleton.Data;

            //Creates a new custom skin
            var customSkin = new Skin("CustomCharacter");


            switch (skinEntity.Job)
            {
                //Combines the skins based on the gear choices
                case Jobs.Warrior:
                {
                    customSkin.AddSkin(skeletonData.FindSkin("MELEE " + skinEntity.Melee));
                    
                    if (skinEntity.Shield == 0)
                    {
                        customSkin.AddSkin(skeletonData.FindSkin("EMPTY"));
                    }
                    else
                    {
                        customSkin.AddSkin(skeletonData.FindSkin("SHIELD " + (skinEntity.Shield -1)));
                    }

                    break;
                }
                case Jobs.Archer:
                {
                    customSkin.AddSkin(skeletonData.FindSkin("BOW "    + skinEntity.Bow));
                    customSkin.AddSkin(skeletonData.FindSkin("QUIVER " + skinEntity.Quiver));
                    break;
                }
                case Jobs.Elementalist:
                {
                    customSkin.AddSkin(skeletonData.FindSkin("STAFF " + skinEntity.Staff));
                    break;
                }
                case Jobs.Duelist:
                {
                    customSkin.AddSkin(skeletonData.FindSkin("OFFHAND " + skinEntity.DuelistOffhand));
                    customSkin.AddSkin(skeletonData.FindSkin("MELEE "   + skinEntity.Melee));
                    break;
                }
            }

            customSkin.AddSkin(skeletonData.FindSkin("ARMOR " + skinEntity.Armor));
            customSkin.AddSkin(skeletonData.FindSkin("ARMOR " + skinEntity.Armor));
            customSkin.AddSkin(skeletonData.FindSkin("ARMOR " + skinEntity.Armor));
            customSkin.AddSkin(skeletonData.FindSkin("ARMOR " + skinEntity.Armor));
            
            if (skinEntity.Helmet == 0)
            {
                customSkin.AddSkin(skeletonData.FindSkin("EMPTY"));
            }
            else
            {
                customSkin.AddSkin(skeletonData.FindSkin("HELMET " + skinEntity.Helmet));
            }
            
            customSkin.AddSkin(skeletonData.FindSkin("SHOULDER " + skinEntity.Shoulder));
            customSkin.AddSkin(skeletonData.FindSkin("ARM "      + skinEntity.Arm));
            customSkin.AddSkin(skeletonData.FindSkin("FEET "     + skinEntity.Feet));
            customSkin.AddSkin(skeletonData.FindSkin("HAIR "     + skinEntity.Hair));
            customSkin.AddSkin(skeletonData.FindSkin("EYES "     + skinEntity.Face));

            //Sets the new created skin on the character SkeletonAnimation
            skeleton.SetSkin(customSkin);
            skeleton.SetSlotsToSetupPose();
            

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
        

        // private void Awake()
        // {
        //     ApplySkinChanges();
        // }
    }
}
