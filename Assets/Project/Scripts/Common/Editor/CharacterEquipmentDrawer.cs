using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class CharacterEquipmentDrawer : OdinAttributeProcessor<CharacterEquipment>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "weaponInfo")
            {
                attributes.Add(new InfoBoxAttribute("Inspector에서 설정하는 값은 에디터 전용이며, 빌드는 SaveManager를 참고합니다.", InfoMessageType.Info));
                attributes.Add(new TitleGroupAttribute("Weapon"));
            }
            
            if (member.Name == "headInfo")
            {
                attributes.Add(new TitleGroupAttribute("Head"));
            }
            
            if (member.Name == "topInfo")
            {
                attributes.Add(new TitleGroupAttribute("Top"));
            }
            
            if (member.Name == "bottomInfo")
            {
                attributes.Add(new TitleGroupAttribute("Bottom"));
            }
            
            if (member.Name == "trinket1Info")
            {
                attributes.Add(new TitleGroupAttribute("Trinket1"));
            }
            
            if (member.Name == "trinket2Info")
            {
                attributes.Add(new TitleGroupAttribute("Trinket2"));
            }
            
            if (member.Name == "equipmentTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
