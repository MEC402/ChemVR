using System;
using UnityEngine;

public class WebGLEvents
{
    public event Action<GameObject> onObjectGrabbed;
    public void ObjectGrabbed(GameObject obj) => onObjectGrabbed?.Invoke(obj);

    public event Action<GameObject> onObjectReleased;
    public void ObjectReleased(GameObject obj) => onObjectReleased?.Invoke(obj);

    public event Action onInteractPressed;
    public void InteractPressed() => onInteractPressed?.Invoke();
}
