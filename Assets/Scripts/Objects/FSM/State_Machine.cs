using UnityEngine;

public class State_Machine
{
    public State CurrentState { get; private set; }

    public State_Machine(State startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();       
    }

    public void ChangeState(State newState)
    {
        CurrentState.Exit();

        CurrentState = newState;
        newState.Enter();        
    }
}
