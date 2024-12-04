using System;

public static class BallEventManager
{
    public static event Action<string> OnBallStateChanged;

    public static void TriggerStateChange(string newState)
    {
        OnBallStateChanged?.Invoke(newState);
    }
}
