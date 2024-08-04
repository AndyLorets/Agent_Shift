public class StateMachine 
{
    public StateBase currentState; 
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
    }
}
