using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clownfish : Fish
{
    public Clownfish() : base(Species.CLOWNNFISH)
    {
        movementSpeed = 5f;
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
