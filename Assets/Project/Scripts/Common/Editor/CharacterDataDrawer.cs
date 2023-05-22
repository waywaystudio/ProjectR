using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class CharacterDataDrawer : OdinAttributeProcessor<CharacterData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "constEntity")
            {
                attributes.Add(new TabGroupAttribute("CharacterEntity", "ConstEntity"));
                attributes.Add(new HideLabelAttribute());
            }
         
            if (member.Name == "equipmentEntity")
            {
                attributes.Add(new HideLabelAttribute());
                attributes.Add(new TabGroupAttribute("CharacterEntity", "Equipment"));
            }
            
            if (member.Name == "StaticStatTable")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new TitleGroupAttribute("Static Stat Table", "Const + Equipment"));
            }
        }
    }
    
    public class CharacterConstEntityDrawer : OdinAttributeProcessor<CharacterConstEntity>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "defaultSkillList")
            {
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    IsReadOnly = true,
                });
            }
        }
    }
    
    public class CharacterEquipmentEntityDrawer : OdinAttributeProcessor<CharacterEquipmentEntity>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "initialEquipmentIndexList")
            {
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    IsReadOnly = true,
                });
            }
            
            if (member.Name == "EquipmentEntities")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new ReadOnlyAttribute());
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    IsReadOnly = true,
                });
            }
            
            if (member.Name == "EquipmentsStatTable")
            {
                attributes.Add(new TitleGroupAttribute("Equipment Stat Sum", "each by each"));
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
