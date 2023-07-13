using System;
using System.Collections.Generic;
using System.Reflection;
using Common.Effects;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace Common.Editor
{
    public class EffectorDrawer : OdinAttributeProcessor<Effector>
    {
        public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
        {
            attributes.Add(new HideLabelAttribute());
        }
        
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            var lds = new ListDrawerSettingsAttribute
            {
                IsReadOnly  = true, 
                ShowFoldout = true,
            };
            
            if (member.Name == "combatParticles")
            {
                attributes.Add(new TitleGroupAttribute("Effector", "sfx, vfx and so on"));
                attributes.Add(new HideIfAttribute("isEmptyCombatParticle"));
                attributes.Add(lds);
            }
            
            if (member.Name == "combatSounds")
            {
                attributes.Add(new TitleGroupAttribute("Effector"));
                attributes.Add(new HideIfAttribute("isEmptyCombatSounds"));
                attributes.Add(lds);
            }
            
            if (member.Name == "combatImpulses")
            {
                attributes.Add(new TitleGroupAttribute("Effector"));
                attributes.Add(new HideIfAttribute("isEmptyCombatImpulses"));
                attributes.Add(lds);
            }
            
            if (member.Name == "combatCameras")
            {
                attributes.Add(new TitleGroupAttribute("Effector"));
                attributes.Add(new HideIfAttribute("isEmptyCombatCameras"));
                attributes.Add(lds);
            }
            
            if (member.Name == "combatPostProcesses")
            {
                attributes.Add(new TitleGroupAttribute("Effector"));
                attributes.Add(new HideIfAttribute("isEmptyCombatPostProcesses"));
                attributes.Add(lds);
            }
            
            if (member.Name == "hitPauses")
            {
                attributes.Add(new TitleGroupAttribute("Effector"));
                attributes.Add(new HideIfAttribute("isEmptyHitPauses"));
                attributes.Add(lds);
            }
            
            if (member.Name == "bulletTimes")
            {
                attributes.Add(new TitleGroupAttribute("Effector"));
                attributes.Add(new HideIfAttribute("isEmptyBulletTimes"));
                attributes.Add(new PropertySpaceAttribute(0f, 10f));
                attributes.Add(lds);
            }
        }
    }
}
