using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnRodState : IState
{
    Hook hook;
    public OnRodState(Hook pablo)
    {
        hook = pablo;
    }
}
