using UnityEngine;

public class FPSForcer : MonoBehaviour
{
    [SerializeField] int targetFPS = 60;

    void Awake()
    {
        Application.targetFrameRate = targetFPS;
    }
}
