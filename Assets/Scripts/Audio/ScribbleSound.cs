using UnityEngine;

public class ScribbleSound : MonoBehaviour
{
    [SerializeField] AudioSource scribbleSound;

    private void Start()
    {
        if (scribbleSound == null)
            scribbleSound = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        AudioEventManager.OnScribbleSound += PlayScribbleSound;
    }

    private void OnDisable()
    {
        AudioEventManager.OnScribbleSound -= PlayScribbleSound;
    }

    private void PlayScribbleSound() => scribbleSound.Play();
}
