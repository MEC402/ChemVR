using System;

public static class AudioEventManager
{
    public static event Action OnDrinkSound;

    public static void DrinkSound() => OnDrinkSound?.Invoke();
}
