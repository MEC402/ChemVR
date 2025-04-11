using System;
using UnityEngine;

public class WebGLEvents
{
    public event Action<GameObject> OnObjectGrabbed;
    public void ObjectGrabbed(GameObject obj) => OnObjectGrabbed?.Invoke(obj);

    public event Action<GameObject> OnObjectReleased;
    public void ObjectReleased(GameObject obj) => OnObjectReleased?.Invoke(obj);

    public event Action<GameObject> OnInteractPressed;
    public void InteractPressed(GameObject obj) => OnInteractPressed?.Invoke(obj);
}
