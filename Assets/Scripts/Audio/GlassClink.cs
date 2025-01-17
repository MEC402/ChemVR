using System.Collections;
using UnityEngine;

public class GlassClink : MonoBehaviour
{
    [SerializeField] AudioSource glassAudio;

    bool canStart = false;

    private void Start()
    {
        if (glassAudio == null)
            glassAudio = GetComponent<AudioSource>();

        StartCoroutine(AudioDelay());
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!canStart) return;

        glassAudio.pitch = Random.Range(0.9f, 1.1f);
        glassAudio.Play();
    }

    IEnumerator AudioDelay()
    {
        yield return new WaitForSeconds(1);
        canStart = true;
    }
}
