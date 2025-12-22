using UnityEngine;

public class AudioPlay : MonoBehaviour
{
    #region Variables
    [SerializeField] AudioSource slapAudio;
    [SerializeField] AudioSource drinkAudio;
    [SerializeField] AudioSource goggleAudio, gloveAudio, labCoatAudio;
    #endregion

    #region Unity Methods
    private void OnEnable()
    {
        GameEventsManager.instance.miscEvents.onPrinterSlap += PlaySlapSound;
        AudioEventManager.OnDrinkSound += PlayDrinkSound;

        AudioEventManager.OnGoggleSound += PlayGoggleSound;
        AudioEventManager.OnGloveSound += PlayGloveSound;
        AudioEventManager.OnLabCoatSound += PlayCoatSound;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onPrinterSlap -= PlaySlapSound;
        AudioEventManager.OnDrinkSound -= PlayDrinkSound;

        AudioEventManager.OnGoggleSound -= PlayGoggleSound;
        AudioEventManager.OnGloveSound -= PlayGloveSound;
        AudioEventManager.OnLabCoatSound -= PlayCoatSound;
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

    private void PlayGoggleSound()
    {
        if (goggleAudio == null)
            return;
        goggleAudio.Play();
    }

    private void PlayGloveSound()
    {
        if (gloveAudio == null)
            return;
        gloveAudio.Play();
    }
    private void PlayCoatSound()
    {
        if (labCoatAudio == null)
            return;
        labCoatAudio.Play();
    }
    #endregion
}
