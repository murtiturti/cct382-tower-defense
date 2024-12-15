using System;
using UnityEngine;

public abstract class ScriptableEvent<T> : ScriptableObject
{
    private Action<T> OnEventRaised;

    public void Raise(T value)
    {
        OnEventRaised?.Invoke(value);
    }

    public void RegisterListener(Action<T> listener)
    {
        OnEventRaised += listener;
    }

    public void UnregisterListener(Action<T> listener)
    {
        OnEventRaised -= listener;
    }
}

[Serializable]
public struct Void { } // A placeholder struct for events without data

[CreateAssetMenu(menuName = "Events/Void Event")]
public class VoidEvent : ScriptableEvent<Void> { }

[CreateAssetMenu(menuName = "Events/Float Event")]
public class FloatEvent : ScriptableEvent<float> { }

[CreateAssetMenu(menuName = "Events/String Event")]
public class StringEvent : ScriptableEvent<string> { }

[CreateAssetMenu(menuName = "Events/Bool Event")]
public class BoolEvent : ScriptableEvent<bool> { }

[CreateAssetMenu(menuName = "Events/GameObject Event")]
public class GameObjectEvent : ScriptableEvent<GameObject> { }