// #if UNITY_EDITOR
// using Sirenix.OdinInspector.Editor;
// #endif
// using System;
// using System.Collections.Generic;
// using MainGame.Manager.Debugging;
// using Sirenix.OdinInspector;
// using UnityEngine;
//
// namespace MainGame.Manager.Debugging
// {
//     public class DebugManager : MonoBehaviour
//     {
//         // [ShowInInspector]
//         // protected static BaseDebug BaseDebug = BaseDebug.Instance;
//         // [ShowInInspector]
//         // protected static SkillDebug SkillDebug = SkillDebug.Instance;
//
//         [SerializeField] private DebugViewer<SkillDebug> skillDebugger = new();
//
//         [Button]
//         private void TestSkillDebug()
//         {
//             SkillDebug.Instance.Log("SkillDebug");
//         }
//
//         // [OnInspectorInit]
//         // private void GetDebugger()
//         // {
//             // debuggerList.Clear();
//             // debuggerList.Add(CombatDebug.Instance);
//             // debuggerList.Add(SkillDebug.Instance);
//             
//             // Finder.TryGetObjectList(out debuggerList);
//         // }
//     }
//
// #if UNITY_EDITOR
//     public class DebugManagerDrawer : OdinAttributeProcessor<DebugManager>
//     {
//         public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
//         {
//             attributes.Add(new TitleAttribute("Control Debug Show by Category", "Set true to show"));
//         }
//     }
// #endif
// }
//
// [Serializable]
// public class DebugViewer<T> where T : BaseDebug<>
// {
//     public DebugViewer()
//     {
//         Debugger.DoNotShow = doNotShow;
//         Debugger.IsShowLog = isShowLog;
//     }
//     
//     protected static T Debugger = BaseDebug<T>.Instance;
//     
//     [OnValueChanged("SetDoNotShow")]
//     public bool doNotShow;
//     public bool isShowLog;
//
//     private void SetDoNotShow() => Debugger.DoNotShow = doNotShow;
//     private void SetIsShowLog() => Debugger.IsShowLog = isShowLog;
// }
//
// [Serializable]
// public class SkillDebug : BaseDebug<SkillDebug> {}