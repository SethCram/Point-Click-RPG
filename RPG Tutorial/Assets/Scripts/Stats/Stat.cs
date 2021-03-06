using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//all fields in class will show up in inspector:
[System.Serializable] 
public class Stat
{
    [SerializeField] //so can edit from inspector
    private int baseValue; //default value of curr stat

    private List<int> modifiers = new List<int>();

    public int GetValue()
    {
        //factor in modifiers here:
        int finalValue = baseValue;

        //tutorial did: modifiers.ForEach(x => finalValue += x ); //x is each val in 'modifiers' list added to 'finalValue'
        foreach (int modValue in modifiers)
        {
            finalValue += modValue;
        }

        //no negative stats:
        if(finalValue < 0)
        {
            finalValue = 0;
        }

        return finalValue;
    }

    public void AddModifier (int mod)
    {
        if(mod != 0)
        {
            modifiers.Add(mod);
        }
    }

    public void RemoveModifier (int mod)
    {
        if(mod != 0)
        {
            modifiers.Remove(mod);
        }
    }
}
