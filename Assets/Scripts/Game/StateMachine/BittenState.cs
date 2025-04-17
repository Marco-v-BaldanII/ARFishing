using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BittenState : IState
{
    Hook hook;
    public BittenState(Hook pablo)
    {
        hook = pablo;
    }
}
