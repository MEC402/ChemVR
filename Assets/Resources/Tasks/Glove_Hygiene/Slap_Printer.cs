using UnityEngine;

public class Slap_Printer : TaskStep
{
    int slapCount, slapsToFinish;

    AudioSource brokenPrinterAudio;

    protected override void SetTaskStepState(string state)
    {
        //Not needed
    }
    void OnEnable()
    {
        GameEventsManager.instance.miscEvents.SetHint(GameObject.Find("printer"));

        brokenPrinterAudio = GameObject.Find("printer").GetComponent<AudioSource>();

        PlayPrinterAudio();

        slapCount = 0;

        // Randomize slapsToFinish
        slapsToFinish = Random.Range(2, 6);

        GameEventsManager.instance.miscEvents.onPrinterSlap += Slap;
    }

    void OnDisable()
    {
        GameEventsManager.instance.miscEvents.onPrinterSlap -= Slap;
    }

    private void Slap()
    {
        slapCount++;

        if (slapCount >= slapsToFinish)
        {
            if (brokenPrinterAudio != null)
                brokenPrinterAudio.Stop();

            FinishTaskStep();
        }
    }

    private void PlayPrinterAudio()
    {
        if (brokenPrinterAudio == null) return;

        brokenPrinterAudio.Play();
    }
}
