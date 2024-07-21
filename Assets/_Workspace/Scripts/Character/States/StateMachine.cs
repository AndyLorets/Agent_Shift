public class StateMachine 
{
    private StateBase _currentState; 
    public void ChangeState(StateBase newState)
    {
        if(_currentState == newState) return;   

        _currentState?.ExitState();
        _currentState = newState;
        _currentState?.EnterState();
    }
}
