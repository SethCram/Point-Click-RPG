using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int maxHealth = 50; //not a stat bc don't want it affected by modifiers
    public int currHealth { get; private set; } //can only set thru this class, but can be retrieved by any class

    public float deathAnimDelay = 2f; //time it takes for death anim to play out

    //character stats:
    public Stat atk;
    public Stat def;
    public Stat stealth;
    public Stat bulk; //each bulk stat gives 5 hp

    //event for w/ health changes (max and curr health needed):
    public event System.Action<int, int> OnHealthChangedCallback; //example of an 'event' that takes multiple args, should update healthUI w/ ever called

    public virtual void Die() //Meant to be overwritten
    {
        //start death anim by calling die() in character animator:
        if (GetComponent<CharacterAnimator>() != null)
        {
            GetComponent<CharacterAnimator>().Die();
        }
        else
        {
            Debug.LogWarning("There's no character animator to call");
        }

        Debug.Log( transform.name + "<color=red> died. </color>");
    }

    private void Awake()
    {
        maxHealth += bulk.GetValue() * 5;

        currHealth = maxHealth;
    }

    private void Update()
    {
        //test out 'TakeDamage' method:
        /*
        if(Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(10);
        }
        */
    }

    //take damage based on enemy atk and my def:
    public void TakeDamage(int enemyAtk)
    {
        //int maxDef = 20;
        int damage;
        float damageFloatPH;

        //calc damage off of enemy atk stat val:
        damage = Random.Range(enemyAtk, enemyAtk * 2);

        //factor def into damage calculation:
        damageFloatPH = damage;
        damageFloatPH -= damageFloatPH * (def.GetValue() * 0.04f); //max damage blocked due to def should be 80%
        damage = (int)damageFloatPH;


        //makes sure damage doesn't go negative and heal:
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        //subtract damage amt from curr health and print it:
        currHealth -= damage;
        Debug.Log(transform.name + " takes " + damage + " damage.");

        //health changed event:
        if(OnHealthChangedCallback != null)
        {
            OnHealthChangedCallback(maxHealth, currHealth);
        }

        if(currHealth <= 0)
        {
            Die();
        }
    }
}
