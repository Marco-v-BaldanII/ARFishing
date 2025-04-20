using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.IO.LowLevel.Unsafe;

public class HookStateMachine : MonoBehaviour
{

    [SerializeField] State[] stateTypes;

    private Dictionary<State, IState> states;

    public State startingState;

    public IState currentState;

    public Hook pablo;

    public State checkState;

    bool initialized = false;

    [SerializeField] public float debugSpeed = 0;

    public event Action<Hook, State> OnTransition;

    // Start is called before the first frame update
    public void Start()
    {
        if (initialized) { return; }
        initialized = true;

        states = new Dictionary<State, IState>();

        foreach (var state in stateTypes)
        {
            IState new_state = null;

            switch (state)
            {
                case State.ON_ROD:
                    new_state = new OnRodState(pablo);
                    break;
                case State.THROWN:
                    new_state = new ThrownState(pablo);
                    break;
                case State.CATCHED:
                    new_state = new CatchedState(pablo);
                    break;
                case State.BITTEN:
                    new_state = new BittenState(pablo);
                    break;
                case State.IN_WATER:
                    new_state = new InWaterState(pablo);
                    break;

            }
            // each state should subscribe to the OnChild Transition
            // therefore when Transitioned is meitted by any state, the exit() and enter() occur automatically
            new_state.Transition += OnChildTransitionEvent;
            states[state] = new_state;
        }

        if (states[startingState] != null)
        {
            currentState = states[startingState];
            states[startingState].Enter();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.gameState != GameState.Playing)
            return;
        currentState?.Process();
    }

    void FixedUpdate()
    {
        currentState?.PhysicsProcess();
    }

    // Call sparingly from non-state classes , be careful
    public void OnChildTransitionEvent(State new_state_type)
    {
        if (GameManager.Instance != null && GameManager.Instance.gameState != GameState.Playing)
            return;
        if (states == null) { Start(); }
        // Add scoring logic: award points when transitioning to CATCHED
        if (new_state_type == State.CATCHED && currentState != states[State.CATCHED])
        {
            Debug.Log("HookStateMachine: Transitioning to CATCHED state, attempting to increment score.");
            int scorePerCatch = 10;
            GameManager.Instance?.IncrementScore(scorePerCatch);
        }
        OnTransition?.Invoke(pablo, new_state_type);
        currentState?.Exit();
        states[new_state_type]?.Enter();
        checkState = new_state_type;
        currentState = states[new_state_type];
        return;
    }


    private void OnChildTransitionEvent(State new_state_type, IState prev_state)
    {
        if (currentState != prev_state) { return; }

        prev_state.Exit();
        OnTransition?.Invoke(pablo, new_state_type);
        states[new_state_type]?.Enter();
        checkState = new_state_type;
        currentState = states[new_state_type];

    }

    private void OnTriggerEnter(Collider collision)
    {
        currentState?.OnAreaEnter(collision);
    }

    private void OnTriggerExit(Collider collision)
    {
        currentState?.OnAreaExit(collision);
    }

    private void OnTriggerStay(Collider collision)
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        currentState?.OnBodyEnter(collision.collider);
    }
    private void OnCollisionStay(Collision collision)
    {
        currentState?.OnBodyStay(collision.collider);
    }

    public State GetCurrentState()
    {
        foreach (var entry in states)
        {
            if (entry.Value == currentState)
            {
                return entry.Key;
            }
        }

        return State.ON_ROD;
    }

}
