using System;

public static class AudioEventManager
{
    public static event Action OnDrinkSound;
    public static void DrinkSound() => OnDrinkSound?.Invoke();

    public static event Action OnScribbleSound;
    public static void ScribbleSound() => OnScribbleSound?.Invoke();

    public static event Action OnPourSound;
    public static void PourSound() => OnPourSound?.Invoke();

    public static event Action OnBubbleSound;
    public static void BubbleSound() => OnBubbleSound?.Invoke();

    public static event Action OnGloveSound;
    public static void GloveSound() => OnGloveSound?.Invoke();

    public static event Action OnGoggleSound;
    public static void GoggleSound() => OnGoggleSound?.Invoke();

    public static event Action OnLabCoatSound;
    public static void LabCoatSound() => OnLabCoatSound?.Invoke();
}
