using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace Common.Editor
{
    public class CharacterCombatStatusDrawer : OdinAttributeProcessor<CharacterCombatStatus>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "Alive")
            {
                attributes.Add(new TitleGroupAttribute("LiveStat", "hp, resource and so on"));
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "Hp")
            {
                attributes.Add(new TitleGroupAttribute("LiveStat"));
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "Resource")
            {
                attributes.Add(new TitleGroupAttribute("LiveStat"));
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "Shield")
            {
                attributes.Add(new TitleGroupAttribute("LiveStat"));
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "StatTable")
            {
                attributes.Add(new TitleGroupAttribute("Dynamic Stat Table", "Static + Buffs"));
                attributes.Add(new ShowInInspectorAttribute());
            }
            
            if (member.Name == "BuffTable")
            {
                attributes.Add(new TitleGroupAttribute("Status Table", "Buff & DeBuff"));
                attributes.Add(new ShowInInspectorAttribute());
            }
            if (member.Name == "DeBuffTable")
            {
                attributes.Add(new TitleGroupAttribute("Status Table"));
                attributes.Add(new ShowInInspectorAttribute());
            }
        }
    }
}
