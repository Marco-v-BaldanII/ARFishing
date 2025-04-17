using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownState : IState
{
    Hook hook;
    public ThrownState(Hook pablo)
    {
        hook = pablo;
    }
}
