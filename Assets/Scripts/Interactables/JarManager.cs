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

        //TODO: Add scoop interaction later
        // if (other.name == "Scoop")
        //     GameEventsManager.instance.miscEvents.ScoopInJar(other.gameObject, gameObject);
    }

    private void CloseJar(GameObject jar, bool closedJar)
    {
        if (gameObject == jar)
            canUse = !closedJar;
    }
}
