using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TROUT : Fish
{


    public TROUT() : base(Species.CLOWNNFISH)
    {
        movementSpeed = 6f;
    }

    protected void Awake()
    {
        base.Awake();
    }
    protected void Update()
    {
        base.Update();
    }

    public override void Action()
    {
        base.Action();
    }
}
