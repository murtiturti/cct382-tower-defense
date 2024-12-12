using UnityEngine;
using UnityEngine.Events;

public class EventListener<T> : MonoBehaviour
{
    [Tooltip("The event to listen for.")]
    [SerializeField] private ScriptableEvent<T> gameEvent;

    [Tooltip("The response to invoke when the event is raised.")]
    [SerializeField] private UnityEvent<T> response;

    // Register the listener
    private void OnEnable()
    {
        if (gameEvent != null)
        {
            gameEvent.RegisterListener(OnEventRaised);
        }
    }

    // Unregister the listener to prevent memory leaks
    private void OnDisable()
    {
        if (gameEvent != null)
        {
            gameEvent.UnregisterListener(OnEventRaised);
        }
    }

    // The method invoked when the event is triggered
    private void OnEventRaised(T value)
    {
        response?.Invoke(value);
    }
}