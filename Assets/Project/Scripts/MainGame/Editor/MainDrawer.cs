using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace MainGame.Editor
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
            if (member.Name == "SetUp")
            {
                // attributes.Add(CommonHorizontalGroup);
                attributes.Add(CommonHorizontalGroup);
                attributes.Add(new ButtonAttribute(ButtonSizes.Large)
                {
                    Icon = SdfIconType.Save,
                });
            }
        }
        
    }
}