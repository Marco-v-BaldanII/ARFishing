using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    ON_ROD,
    THROWN,
    BITTEN,
    CATCHED,
    IN_WATER
}

public class IState
{

    public delegate void TransitionEvent(State new_state_type, IState prev_state);

    public event TransitionEvent Transition;

    // Events in C# can only be called from the class that owns them, other classes can only subscribe or unsubscribe
    public virtual void CallTransition(State new_state_type, IState prev_) { Transition.Invoke(new_state_type, prev_); }

    // Start method
    public virtual void Enter() { }

    public virtual void Exit() { }

    // Update method
    public virtual void Process() { }

    public virtual void PhysicsProcess() { }

    //On trigger etner
    public virtual void OnAreaEnter(Collider collision) { }
    //On trigger stay
    public virtual void OnAreaStay(Collider collision) { }
    //On trigger exit
    public virtual void OnAreaExit(Collider collision) { }
    //On collision enter
    public virtual void OnBodyEnter(Collider collison) { }
    //On Collision stay
    public virtual void OnBodyStay(Collider collison) { }
    // No me apetecía hacer un On Collision exit >:v

}
