using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    #region Variables
    [SerializeField] AudioSource slapAudio;
    [SerializeField] AudioSource drinkAudio;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onPrinterSlap += PlaySlapSound;
        AudioEventManager.OnDrinkSound += PlayDrinkSound;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onPrinterSlap -= PlaySlapSound;
        AudioEventManager.OnDrinkSound -= PlayDrinkSound;
    }
    #endregion

    #region Custom Methods
    /// <summary>
    /// Play the slap sound
    /// </summary>
    private void PlaySlapSound()
    {
        if (slapAudio == null) return;

        slapAudio.pitch = Random.Range(0.8f, 1.2f); // Randomize pitch to make it sound more natural

        slapAudio.Play();
    }

    /// <summary>
    /// Play the drink sound
    /// </summary>
    private void PlayDrinkSound()
    {
        if (drinkAudio == null) return;

        drinkAudio.pitch = Random.Range(0.9f, 1.1f); // Randomize pitch to make it sound more natural

        drinkAudio.Play();
    }
    #endregion
}
