using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HygieneManager : MonoBehaviour
{
    [SerializeField]
    ParticleSystem ps;
    [SerializeField]
    List<Vector3> points;
    List<Vector3> localPoints;
    List<GameObject> gameObjects;
    [SerializeField]
    bool showPoints = false;
    [SerializeField]
    bool forceUpdatePoints = true;

    ParticleSystem.Particle[] particles;

    private void OnValidate() {
        ps = GetComponent<ParticleSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        points = new List<Vector3>();
        localPoints = new List<Vector3>();
        gameObjects = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!showPoints) {
            return;
        }

        InitIfNeeded();
        int numParticles = ps.GetParticles(particles);
        if (points.Count < numParticles) { 
            numParticles = points.Count; 
        }

        for (int i = 0; i < numParticles; i++) {
            particles[i].velocity = Vector3.zero;
            particles[i].position = points[i];
        }

        ps.SetParticles(particles, numParticles);
    }

    void InitIfNeeded() {
        if (ps == null) {
            ps = GetComponent<ParticleSystem>();
        }
        if (particles == null || particles.Length < ps.main.maxParticles) { 
            particles = new ParticleSystem.Particle[ps.main.maxParticles];
        }
        if (points.Count < localPoints.Count || forceUpdatePoints) {
            points = new List<Vector3>();
            for (int i = 0; i < localPoints.Count; i++) {
                points.Add(gameObjects[i].transform.TransformPoint(localPoints[i]));
            }
        }
    }

    public void AddPoint(Vector3 point, GameObject gameObject) {
        localPoints.Add(point);
        gameObjects.Add(gameObject);
    }
}
