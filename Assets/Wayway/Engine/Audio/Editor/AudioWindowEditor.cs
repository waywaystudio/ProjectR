﻿using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;

namespace Wayway.Engine.Audio.Editor
{
    public class AudioWindowEditor : OdinMenuEditorWindow
    {
        private List<AudioClipData> audioClipDataList;

        [MenuItem("Wayway/AudioManager")]
        public static void OpenWidow()
        {
            var window = GetWindow<AudioWindowEditor>();

            window.minSize = new Vector2(800f, 800f);
            window.MenuWidth = 280f;
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            audioClipDataList = ScriptableObjectUtility.GetScriptableObjectList<AudioClipData>();

            var tree = new OdinMenuTree(supportsMultiSelect: true)
            {
                { "BGM ClipData", new AudioClipDataDrawer(audioClipDataList.FindAll(x => x.Type == AudioType.Bgm)), EditorIcons.Sound },
                { "SFX ClipData", new AudioClipDataDrawer(audioClipDataList.FindAll(x => x.Type == AudioType.Sfx)), EditorIcons.Sound },
                { "Bumper ClipData", new AudioClipDataDrawer(audioClipDataList.FindAll(x => x.Type == AudioType.Bumper)), EditorIcons.Sound },
                { "UnSorted", new AudioClipDataDrawer(audioClipDataList.FindAll(x => x.Type == AudioType.None)), EditorIcons.ConsoleErroricon },
            };
            
            tree.DefaultMenuStyle.Height = 40;
            tree.DefaultMenuStyle.IconSize = 20;
            tree.DefaultMenuStyle.IconOffset = -2;

            return tree;
        }

        public class AudioClipDataDrawer
        {
            public AudioClipDataDrawer(List<AudioClipData> clips)
            {
                Clips = clips;
            }
            
            [TableList(AlwaysExpanded = true, HideToolbar = true)] 
            public List<AudioClipData> Clips;
        }
    }
}

