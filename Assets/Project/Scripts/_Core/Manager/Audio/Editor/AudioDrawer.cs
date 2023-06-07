#if ODIN_INSPECTOR
using System;
using System.Collections.Generic;
using System.Reflection;
using Manager.Audio;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

namespace MainGame.Manager.Audio.Editor
{
    public class AudioManagerDrawer : OdinAttributeProcessor<AudioManager>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "masterMixer")
            {
                attributes.Add(new PropertySpaceAttribute(0, 15));
                attributes.Add(new RequiredAttribute());
            }
            
            if (member.Name == "audioChannels")
            {
                attributes.Add(new ListDrawerSettingsAttribute
                {
                    ShowFoldout = true,
                    IsReadOnly = true
                });
            }

            if (member.Name == "GetAudioChannels")
            {
                attributes.Add(new ButtonAttribute(ButtonSizes.Large));
            }
        }
    }

    public class AudioClipDataDrawer : OdinAttributeProcessor<AudioClipData>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "PlayPreviewClip")
            {
                attributes.Add(new HorizontalGroupAttribute("Preview"));
                attributes.Add(new ButtonAttribute(name: " "));
                attributes.Add(new AudioIconAttribute("Play"));
            }
            if (member.Name == "StopPreviewClip")
            {
                attributes.Add(new HorizontalGroupAttribute("Preview"));
                attributes.Add(new ButtonAttribute(name: " "));
                attributes.Add(new AudioIconAttribute("Stop"));
            }
        }
    }
    
    public class AudioPlayableDrawer : OdinAttributeProcessor<IAudioPlayable>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "type")
            {
                attributes.Add(new TableColumnWidthAttribute(100, false));
            }
            if (member.Name == "audioClip")
            {
                attributes.Add(new TableColumnWidthAttribute(150));
            }
            if (member.Name == "priority")
            {
                attributes.Add(new PropertyRangeAttribute(0, 256));
                attributes.Add(new TableColumnWidthAttribute(150));
            }
        }
    }

    public class AudioClipDataGroupDrawer : OdinAttributeProcessor<AudioClipRandomGroup>
    {
        public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member, List<Attribute> attributes)
        {
            if (member.Name == "clipList")
            {
                attributes.Add(new TableListAttribute());
            }
            if (member.Name == "pickedClip")
            {
                attributes.Add(new ShowInInspectorAttribute());
                attributes.Add(new ReadOnlyAttribute());
            }
            
            if (member.Name == "PlayPreviewClip")
            {
                attributes.Add(new HorizontalGroupAttribute("Preview"));
                attributes.Add(new ButtonAttribute(name: " "));
                attributes.Add(new AudioIconAttribute("Play"));
            }
            if (member.Name == "StopPreviewClip")
            {
                attributes.Add(new HorizontalGroupAttribute("Preview"));
                attributes.Add(new ButtonAttribute(name: " "));
                attributes.Add(new AudioIconAttribute("Stop"));
            }
        }

    }
}
#endif
