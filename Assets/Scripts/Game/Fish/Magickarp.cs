using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MAGIKARP : Fish
{


    public MAGIKARP() : base(Species.CLOWNNFISH)
    {
        movementSpeed = 10f;
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
