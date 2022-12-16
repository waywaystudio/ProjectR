// #if UNITY_EDITOR
// using System.Reflection;
// using Sirenix.OdinInspector.Editor;
// #endif
// using System;
// using System.Collections.Generic;
// using System.Diagnostics;
// using Core.Singleton;
// using Sirenix.OdinInspector;
// using UnityEngine;
// using Debug = UnityEngine.Debug;
//
// public interface IDebugger
// {
//     bool DoNotShow { get; set; }
//     bool IsShowLog { get; set; }
// }
//
// #if !DEVELOPMENT_BUILD && !FORCE_LOGGING
// namespace MainGame.Manager.Debugging
// {
//     [Serializable]
//     public class BaseDebug<T> : Singleton<T>, IDebugger where T : class, new()
//     {
//         // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
//         // private static void Init()
//         // {
//         //     Debug.Log("BaseDebug Reset");
//         //     Instance = null;
//         // }
//
//         public bool doNotShow;
//         public bool isShowLog = true;
//         [SerializeField] protected bool isShowWarning = true;
//         [SerializeField] protected bool isShowError = true;
//         [SerializeField] protected bool isShowElse = true;
//
//         public bool DoNotShow { get => doNotShow; set => doNotShow = value; }
//         public bool IsShowLog { get => isShowLog && !doNotShow; set => isShowLog = value; }
//         public bool IsShowError => isShowError && !doNotShow;
//         public bool IsShowWarning => isShowWarning && !doNotShow;
//         public bool IsShowElse => isShowElse && !doNotShow;
//         
//         public bool IsDebugBuild => Debug.isDebugBuild;
//         public ILogger Logger = Debug.unityLogger;
//         public ILogger UnityLogger => Debug.unityLogger;
//
//         #region Debug::LogType
//
//         [Conditional("UNITY_EDITOR")]
//         public void Log(object message)
//         {
//             if (IsShowLog)
//             {
//                 Debug.Log(message);
//             }
//         }
//
//         // [Conditional("UNITY_EDITOR")]
//         // public static void Log(object message, Object context)
//         // {
//         //     if (IsShowLog) Debug.Log(message, context);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogFormat(string message, params object[] args)
//         // {
//         //     if (IsShowLog) Debug.LogFormat(message, args);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogFormat(Object context, string message, params object[] args)
//         // {
//         //     if (IsShowLog) Debug.LogFormat(message, args);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogWarning(object message)
//         // {
//         //     if (IsShowWarning) Debug.LogWarning(message);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogWarning(object message, Object context)
//         // {
//         //     if (IsShowWarning) Debug.LogWarning(message, context);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogWarningFormat(string format, params object[] args)
//         // {
//         //     if (IsShowWarning) Debug.LogWarningFormat(format, args);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogWarningFormat(Object context, string format, params object[] args)
//         // {
//         //     if (IsShowWarning) Debug.LogWarningFormat(context, format, args);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogError(object message)
//         // {
//         //     if (IsShowError) Debug.LogError(message);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogError(object message, Object context)
//         // {
//         //     if (IsShowError) Debug.LogError(message, context);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogErrorFormat(string message, params object[] args)
//         // {
//         //     if (IsShowError) Debug.LogErrorFormat(message, args);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogErrorFormat(Object context, string message, params object[] args)
//         // {
//         //     if (IsShowError) Debug.LogErrorFormat(message, args);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogException(Exception exception)
//         // {
//         //     Debug.LogException(exception);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogException(Exception exception, Object context)
//         // {
//         //     Debug.LogException(exception, context);
//         // }
//         //
//         #endregion
//         //
//         //
//         // #region Debug::BreakType
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void Break()
//         // {
//         //     if (IsShowElse) Debug.Break();
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void DebugBreak()
//         // {
//         //     if (IsShowElse) Debug.DebugBreak();
//         // }
//         //
//         // #endregion
//         // #region Debug::DrawType
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void DrawLine(Vector3 start, Vector3 end)
//         // {
//         //     if (IsShowElse) Debug.DrawLine(start, end);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void DrawLine(Vector3 start, Vector3 end, Color color)
//         // {
//         //     if (IsShowElse) Debug.DrawLine(start, end, color);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
//         // {
//         //     if (IsShowElse) Debug.DrawLine(start, end, color, duration);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
//         // {
//         //     if (IsShowElse) Debug.DrawLine(start, end, color, duration, depthTest);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void DrawRay(Vector3 start, Vector3 dir)
//         // {
//         //     if (IsShowElse) Debug.DrawRay(start, dir);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void DrawRay(Vector3 start, Vector3 dir, Color color)
//         // {
//         //     if (IsShowElse) Debug.DrawRay(start, dir, color);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
//         // {
//         //     if (IsShowElse) Debug.DrawRay(start, dir, color, duration);
//         // }
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
//         // {
//         //     if (IsShowElse) Debug.DrawRay(start, dir, color, duration, depthTest);
//         // }
//         //
//         // #endregion
//         // #region Debug::AssertType
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void Assert(bool condition)
//         // {
//         //     if (IsShowElse) Debug.Assert(condition);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void Assert(bool condition, Object context)
//         // {
//         //     if (IsShowElse) Debug.Assert(condition, context);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void Assert(bool condition, object message)
//         // {
//         //     if (IsShowElse) Debug.Assert(condition, message);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void Assert(bool condition, string message)
//         // {
//         //     if (IsShowElse) Debug.Assert(condition, message);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void Assert(bool condition, object message, Object context)
//         // {
//         //     if (IsShowElse) Debug.Assert(condition, message, context);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void Assert(bool condition, string message, Object context)
//         // {
//         //     if (IsShowElse) Debug.Assert(condition, message, context);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void AssertFormat(bool condition, string format, params object[] args)
//         // {
//         //     if (IsShowElse) Debug.AssertFormat(condition, format, args);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void AssertFormat(bool condition, Object context, string format, params object[] args)
//         // {
//         //     if (IsShowElse) Debug.AssertFormat(condition, context, format, args);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogAssertion(object message)
//         // {
//         //     if (IsShowElse) Debug.LogAssertion(message);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogAssertion(object message, Object context)
//         // {
//         //     if (IsShowElse) Debug.LogAssertion(message, context);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogAssertionFormat(string format, params object[] args)
//         // {
//         //     if (IsShowElse) Debug.LogAssertionFormat(format, args);
//         // }
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // public static void LogAssertionFormat(Object context, string format, params object[] args)
//         // {
//         //     if (IsShowElse) Debug.LogAssertionFormat(context, format, args);
//         // }
//         //
//         // #endregion
//         //
//         // [Conditional("UNITY_EDITOR")]
//         // public static void ClearDeveloperConsole()
//         // {
//         //     if (IsShowElse) Debug.ClearDeveloperConsole();
//         // }
//
//         #region OBSOLETE
//
//         // [Obsolete("Debug.logger is obsolete. Please use Debug.unityLogger instead (UnityUpgradable) -> unityLogger")]
//         // public static ILogger logger => UnityEngine.Debug.logger;
//         //
//         // [Conditional("UNITY_ASSERTIONS")]
//         // [Conditional("UNITY_EDITOR")]
//         // [Obsolete("Assert(bool, string, params object[]) is obsolete. Use AssertFormat(bool, string, params object[]) (UnityUpgradable) -> AssertFormat(*)", true)]
//         // public static void Assert(bool condition, string format, params object[] args)
//         // {
//         //     if (Instance.ShowDebug) Debug.Assert(condition, format, args);
//         // }
//
//         #endregion
//     }
//     
// #if UNITY_EDITOR
//     public class BaseDebugDrawer<T0, T1> : OdinAttributeProcessor<T0> where T0 : BaseDebug<T1> where T1 : class, new()
//     {
//         public override void ProcessSelfAttributes(InspectorProperty property, List<Attribute> attributes)
//         {
//             attributes.Add(new InlinePropertyAttribute());
//             attributes.Add(new LabelWidthAttribute(140));
//             attributes.Add(new PropertySpaceAttribute(0, 0));
//         }
//     
//         public override void ProcessChildMemberAttributes(InspectorProperty parentProperty, MemberInfo member,
//         List<Attribute> attributes)
//         {
//             attributes.Add(new FoldoutGroupAttribute("Fold"));
//
//             switch (member.Name)
//             {
//                 case "doNotShow":
//                     attributes.Add(new LabelTextAttribute("Do Not Show"));
//                     attributes.Add(new LabelWidthAttribute(90));
//                     break;
//
//                 case "isShowLog":
//                     attributes.Add(new LabelWidthAttribute(200));
//                     attributes.Add(new IndentAttribute(2));
//                     attributes.Add(new HideIfAttribute("doNotShow"));
//                     attributes.Add(new LabelTextAttribute("Common LogType"));
//                     break;
//
//                 case "isShowWarning":
//                     attributes.Add(new LabelWidthAttribute(200));
//                     attributes.Add(new IndentAttribute(2));
//                     attributes.Add(new HideIfAttribute("doNotShow"));
//                     attributes.Add(new LabelTextAttribute("Warning LogType"));
//                     break;
//
//                 case "isShowError":
//                     attributes.Add(new LabelWidthAttribute(200));
//                     attributes.Add(new IndentAttribute(2));
//                     attributes.Add(new HideIfAttribute("doNotShow"));
//                     attributes.Add(new LabelTextAttribute("Error LogType"));
//                     break;
//
//                 case "isShowElse":
//                     attributes.Add(new LabelWidthAttribute(200));
//                     attributes.Add(new IndentAttribute(2));
//                     attributes.Add(new HideIfAttribute("doNotShow"));
//                     attributes.Add(new LabelTextAttribute("Assert, Draw and Break"));
//                     break;
//             }
//         }
//     }
// #endif
// }
// #endif