using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABYSSAL : Fish
{


    public ABYSSAL() : base(Species.CLOWNNFISH)
    {
        movementSpeed = 2f;
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
