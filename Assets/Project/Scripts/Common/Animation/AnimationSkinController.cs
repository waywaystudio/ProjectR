using Sirenix.OdinInspector;
using Spine;
using Spine.Unity;
using UnityEditor;
using UnityEngine;
// ReSharper disable IdentifierTypo

namespace Common.Animation
{
    public enum Jobs
    {
        Warrior, Archer, Elementalist, Duelist
    }
    
    public class AnimationSkinController : MonoBehaviour
    {
        [SerializeField] private SkeletonAnimation skeletonAnimation;
        [SerializeField] private Jobs Job;
        [SerializeField] private int Melee;
        [SerializeField] private int Shield;
        [SerializeField] private int Bow;
        [SerializeField] private int Quiver;
        [SerializeField] private int Staff;
        [SerializeField] private int DuelistOffhand;
        [SerializeField] private int Armor;
        [SerializeField] private int Helmet;
        [SerializeField] private int Shoulder;
        [SerializeField] private int Arm;
        [SerializeField] private int Feet;
        [SerializeField] private int Hair;
        [SerializeField] private int Face;
        [SerializeField] private bool IsAutoUpdate;
        
        [Button]
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


            switch (Job)
            {
                //Combines the skins based on the gear choices
                case Jobs.Warrior:
                {
                    customSkin.AddSkin(skeletonData.FindSkin("MELEE " + Melee));
                    
                    if (Shield == 0)
                    {
                        customSkin.AddSkin(skeletonData.FindSkin("EMPTY"));
                    }
                    else
                    {
                        customSkin.AddSkin(skeletonData.FindSkin("SHIELD " + (Shield -1)));
                    }

                    break;
                }
                case Jobs.Archer:
                {
                    customSkin.AddSkin(skeletonData.FindSkin("BOW "    + Bow));
                    customSkin.AddSkin(skeletonData.FindSkin("QUIVER " + Quiver));
                    break;
                }
                case Jobs.Elementalist:
                {
                    customSkin.AddSkin(skeletonData.FindSkin("STAFF " + Staff));
                    break;
                }
                case Jobs.Duelist:
                {
                    customSkin.AddSkin(skeletonData.FindSkin("OFFHAND " + DuelistOffhand));
                    customSkin.AddSkin(skeletonData.FindSkin("MELEE "   + Melee));
                    break;
                }
            }

            customSkin.AddSkin(skeletonData.FindSkin("ARMOR " + Armor));
            customSkin.AddSkin(skeletonData.FindSkin("ARMOR " + Armor));
            customSkin.AddSkin(skeletonData.FindSkin("ARMOR " + Armor));
            customSkin.AddSkin(skeletonData.FindSkin("ARMOR " + Armor));
            
            if (Helmet == 0)
            {
                customSkin.AddSkin(skeletonData.FindSkin("EMPTY"));
            }
            else
            {
                customSkin.AddSkin(skeletonData.FindSkin("HELMET " + Helmet));
            }
            
            customSkin.AddSkin(skeletonData.FindSkin("SHOULDER " + Shoulder));
            customSkin.AddSkin(skeletonData.FindSkin("ARM " + Arm));
            customSkin.AddSkin(skeletonData.FindSkin("FEET " + Feet));
            customSkin.AddSkin(skeletonData.FindSkin("HAIR " + Hair));
            customSkin.AddSkin(skeletonData.FindSkin("EYES " + Face));

            //Sets the new created skin on the character SkeletonAnimation
            skeleton.SetSkin(customSkin);
            skeleton.SetSlotsToSetupPose();
            

#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
