using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    ON_ROD,
    THROWN,
    BITTEN,
    CATCHED
}

public class IState
{

    public delegate void TransitionEvent(State new_state_type, IState prev_state);

    public event TransitionEvent Transition;

    // Events in C# can only be called from the class that owns them, other classes can only subscribe or unsubscribe
    public virtual void CallTransition(State new_state_type, IState prev_) { Transition.Invoke(new_state_type, prev_); }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Process() { }

    public virtual void PhysicsProcess() { }

    public virtual void OnAreaEnter(Collider collision) { }
    public virtual void OnAreaStay(Collider collision) { }
    public virtual void OnAreaExit(Collider collision) { }

    public virtual void OnBodyEnter(Collider collison) { }
    public virtual void OnBodyStay(Collider collison) { }

}
