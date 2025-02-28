using UnityEngine;

public class JarManager : MonoBehaviour
{
    bool canUse;

    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.OnJarClosed += CloseJar;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.OnJarClosed -= CloseJar;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canUse) return;

        if (other.name.Contains("coopula"))
            GameEventsManager.instance.miscEvents.ScoopInJar();
    }

    private void CloseJar(GameObject jar, bool closedJar)
    {
        if (gameObject == jar)
            canUse = !closedJar;
    }
}
