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
    
    public static void AddPersistantListenerInEditor<T>(this UnityEvent<T> unityEvent, object targetObject, string methodName)
    {
        try
        {
            var activeAction = (UnityAction<T>)Delegate.CreateDelegate(typeof(UnityAction<T>), targetObject, methodName);
            UnityEventTools.AddPersistentListener(unityEvent, activeAction);
        }
        
        // Parameter가 없는 이벤트를 등록할 때.
        catch (ArgumentException)
        {
            var activeAction = (UnityAction)Delegate.CreateDelegate(typeof(UnityAction), targetObject, methodName);
            UnityEventTools.AddVoidPersistentListener(unityEvent, activeAction);
        }
    }

    public static void ClearAllUnityEventInEditor<T>(this UnityEvent<T> unityEvent)
    {
        for (var i = unityEvent.GetPersistentEventCount() - 1; i >= 0; i--) 
            UnityEventTools.RemovePersistentListener(unityEvent, i);
    }
    
    public static void ClearUnityEventInEditor<T>(this UnityEvent<T> unityEvent, string methodName)
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
