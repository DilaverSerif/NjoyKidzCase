using UnityEngine;

public abstract class Settings<T> : ScriptableObject where T : ScriptableObject
{
    public static T Current => Resources.Load<T>("Settings/"+typeof(T).Name);
}

