using UnityEngine;

public abstract class State
{
    protected Connector  _connector;

    public State(Connector connector)
    {
        _connector = connector;       
    }

    abstract public void Enter();
    abstract public void Exit();

    abstract public void SelectConector();
    abstract public void HighlightMode(bool isUnder);

}
