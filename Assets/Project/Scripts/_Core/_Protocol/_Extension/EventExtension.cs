#if UNITY_EDITOR
using UnityEditor.Events;
#endif
using System;
using UnityEngine.Events;

public static class EventExtension
{
#if UNITY_EDITOR
    public static void AddPersistantListenerInEditor(this UnityEvent unityEvent, object targetObject, string methodName)
    {
        var activeAction = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), targetObject, methodName);
        
        UnityEventTools.AddPersistentListener(unityEvent, activeAction);
    }

    public static void ClearAllUnityEventInEditor(this UnityEvent unityEvent)
    {
        for (var i = unityEvent.GetPersistentEventCount() - 1; i >= 0; i--) 
            UnityEventTools.RemovePersistentListener(unityEvent, i);
    }
    
    public static void ClearUnityEventInEditor(this UnityEvent unityEvent, string methodName)
    {
        for (var i = unityEvent.GetPersistentEventCount() - 1; i >= 0; i--)
        {
            if (unityEvent.GetPersistentMethodName(i) == methodName)
            {
                UnityEventTools.RemovePersistentListener(unityEvent, i);
            }
        }
    }
    
#endif
}
