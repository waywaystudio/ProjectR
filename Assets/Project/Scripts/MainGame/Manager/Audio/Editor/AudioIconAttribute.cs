#if ODIN_INSPECTOR
using System;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;

namespace Main.Manager.Audio.Editor
{
    public class AudioIconAttribute : Attribute
    {
        public readonly string Icon;

        public AudioIconAttribute(string icon = "Transparent")
        {
            Icon = icon;
        }

        public EditorIcon Get()
        {
            var prop = typeof(EditorIcons).GetProperty(Icon);
            if (prop != null)
            {
                return (EditorIcon)prop.GetValue(null, null);
            }

            return EditorIcons.Transparent;
        }
        
        public sealed class AudioIconAttributeDrawer : OdinAttributeDrawer<AudioIconAttribute>
        {
            protected override void DrawPropertyLayout(GUIContent label)
            {
                var attribute = Attribute;
                CallNextDrawer(label);
                var rect = GUIHelper.GetCurrentLayoutRect();
                attribute.Get().Draw(rect.Expand(-2), 16);
            }
        }
    }
}
#endif
