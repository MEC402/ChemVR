using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HygieneManager : MonoBehaviour
{
    [SerializeField]
    bool showPoints = false;
    [SerializeField]
    bool trackPoints = false;
    [SerializeField]
    GameObject particlePrefab;
    [SerializeField]
    float particleSize = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        showPoints = false;
        trackPoints = false;
    }

    //In the long run I'd like to remove this
    private void Update()
    {
        ShowPoints(showPoints);
    }

    public void AddPoint(Vector3 point, GameObject gameObject)
    {
        if (trackPoints)
        {
            if (gameObject.name.Contains("XR Origin (XR Rig)") || gameObject.CompareTag("Player"))
            {
                // Ensure that you do not attach chemical sphere's to your XR Origin collider
                return;
            }
            // Instantiate the prefab on the object
            GameObject newObject = Instantiate(particlePrefab, gameObject.transform);

            Vector3 inverseScale = new Vector3(
                1 / gameObject.transform.localScale.x,
                1 / gameObject.transform.localScale.y,
                1 / gameObject.transform.localScale.z
            );
            Vector3 finalScale = inverseScale * particleSize;
            newObject.transform.localScale = finalScale;

            newObject.transform.position = point;
        }
    }

    public void Restart()
    {
        showPoints = false;
        trackPoints = true;
        GameEventsManager.instance.partEvents.DeleteParticles();
    }

    public void ShowPoints(bool toggle)
    {
        showPoints = toggle;
        if (toggle)
        {
            GameEventsManager.instance.partEvents.ShowParticles();
        }
        else
        {
            GameEventsManager.instance.partEvents.HideParticles();
        }
    }

    public void TrackPoints(bool toggle)
    {
        trackPoints = toggle;
    }
}
