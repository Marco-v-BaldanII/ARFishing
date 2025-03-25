using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clownfish : Fish
{
    public Clownfish() : base(Species.CLOWNNFISH)
    {
        movementSpeed = 5f;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void Action()
    {
        base.Action();
    }
}
