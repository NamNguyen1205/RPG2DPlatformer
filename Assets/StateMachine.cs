using UnityEngine;

public class StateMachine
{
    public EntityStatus currentState { get; private set; }

    public void Initialize(EntityStatus startState)
    {
        currentState = startState;
        currentState.Enter();
    }

    public void ChangeState(EntityStatus newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void UpdateActiveState()
    {
        currentState.Update();
    }
}
