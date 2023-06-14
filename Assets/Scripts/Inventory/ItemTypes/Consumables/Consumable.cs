using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Consumable : MonoBehaviour
{
    public MoonPhases moonPhase;
    public TypeConsumable typeConsumable;
    public int charge;

    public string GetDescriptionMoon()
    {
        string description = "";
        switch (moonPhase)
        {
            case MoonPhases.NewMoon:
                description = "Lua Nova";
                break;
            case MoonPhases.FirstQuarter:
                description = "Lua Crescente";
                break;
            case MoonPhases.FullMoon:
                description = "Lua Cheia";
                break;
            case MoonPhases.ThirdQuarter:
                description = "Lua Minguante";
                break;
        }
        return description;
    }
    public abstract void Use();
}

public enum TypeConsumable
{
    Life,
    Energy,
}
