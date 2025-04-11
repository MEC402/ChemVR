using UnityEngine;

public class DisableHoldable : MonoBehaviour
{
    /// <summary>
    /// Disables the holdable object by changing its layer to default
    /// </summary>
    public void Disable() => gameObject.layer = 0;

}
