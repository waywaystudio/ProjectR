using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// Project 안에서 사용되는 모든 프로퍼티에 공통으로 부여하는 OdinAttributeProcessor 클래스. 
    /// </summary>
    public class MainDrawer : OdinAttributeProcessor
    {
        public static readonly HorizontalGroupAttribute CommonHorizontalGroup = new("CommonHorizontal")
        {
            Title = "Set Up Group",
        };

        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "EditorSetUp")
            {
                attributes.Add(CommonHorizontalGroup);
                attributes.Add(new PropertyOrderAttribute(0f));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                {
                    Icon = SdfIconType.Save,
                });
            }
            
            if (member.Name == "ShowDataBase")
            {
                attributes.Add(CommonHorizontalGroup);
                attributes.Add(new PropertyOrderAttribute(1f));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                {
                    Icon = SdfIconType.ClipboardData,
                });
            }
            
            if (member.Name == "Test")
            {
                attributes.Add(new PropertySpaceAttribute(15f, 5f));
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                {
                    Icon = SdfIconType.QuestionCircle,
                    Stretch = false,
                });
            }
        }
    }
    
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
    public class ShowInPlayModeAttribute : Attribute {}
        
    public class ShowInPlayModeAttributeDrawer : OdinAttributeDrawer<ShowInPlayModeAttribute>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            if (EditorApplication.isPlaying)
            {
                CallNextDrawer(label);
            }
        }
    }
}