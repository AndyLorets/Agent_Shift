public class StateMachine 
{
    public StateBase currentState { get; private set; } 
    public void ChangeState(StateBase newState)
    {
        if(currentState == newState) return;   

        currentState?.ExitState();
        currentState = newState;
        currentState?.EnterState();
    }
    public void ExitActiveState()
    {
        currentState?.ExitState();
        currentState = null;
    }
}
