using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static event Action OnClosedRequest;
    public void RaiseCloseEvent()
    {
        Debug.Log("EventHandler: Raising OnClosedRequest event.");
        OnClosedRequest?.Invoke();
    }
}
