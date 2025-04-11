using UnityEngine;

public class SolutionBubbleAudio : MonoBehaviour
{
    [SerializeField] AudioSource bubbleAudio;

    private void Start()
    {
        if (bubbleAudio == null)
            bubbleAudio = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        AudioEventManager.OnBubbleSound += PlayBubbleSound;
    }

    private void OnDisable()
    {
        AudioEventManager.OnBubbleSound -= PlayBubbleSound;
    }

    private void PlayBubbleSound()
    {
        if (bubbleAudio != null)
            bubbleAudio.Play();
    }
}
