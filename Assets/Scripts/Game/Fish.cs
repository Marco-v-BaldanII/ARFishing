using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Species
{
    CLOWNNFISH,
    MAGIKARP,
    ABYSSAL,
    TROUT,
    PABLO
}

public abstract class Fish : MonoBehaviour
{
    protected float movementSpeed;
    protected Species species;
    protected int numberOfActions;

    public Fish(Species species)
    {
        this.species = species;
    }

    public virtual void Action()
    {
        //Perform action
    }

    public Species GetSpecies()
    {
        return species;
    }
}
