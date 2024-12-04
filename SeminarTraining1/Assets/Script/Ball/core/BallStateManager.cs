public class BallStateManager
{
    public enum BallState { Neutral, Static, Dynamic, Decelerating }

    public BallState CurrentState { get; private set; } = BallState.Neutral;

    public bool SetState(BallState newState)
    {
        if (IsValidStateTransition(CurrentState, newState))
        {
            CurrentState = newState;
            return true;
        }
        return false;
    }

    private bool IsValidStateTransition(BallState fromState, BallState toState)
    {
        switch (fromState)
        {
            case BallState.Neutral:
                return toState == BallState.Dynamic;
            case BallState.Dynamic:
                return toState == BallState.Decelerating;
            case BallState.Decelerating:
                return toState == BallState.Static;
            case BallState.Static:
                return false;
        }
        return false;
    }
}
