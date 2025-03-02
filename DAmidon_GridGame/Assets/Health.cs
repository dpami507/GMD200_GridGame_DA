using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int health;
    int maxHealth = 3;

    public bool dead;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if(health <= 0)
            dead = true;
        else dead = false;
    }

    public void TakeDamage()
    {
        health--;
    }
}
