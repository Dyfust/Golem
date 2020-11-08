using UnityEngine;

public static class DebugWrapper
{
    public static void Log(object message)
    {
        #if UNITY_EDITOR
        Debug.Log("Debug Wrapper: " + message);
        #endif
    }
}