using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchedState : IState
{
    Hook hook;
    public CatchedState(Hook pablo)
    {
        hook = pablo;
    }
}
