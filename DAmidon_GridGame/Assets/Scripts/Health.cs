using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public int health;
    int maxHealth = 3;

    public bool dead;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        //Death check
        if(health <= 0)
            dead = true;
        else dead = false;

        if(dead)
        {
            GameManager.instance.inPlay = false;
            this.gameObject.SetActive(false);
        }
    }

    public void TakeDamage()
    {
        health--;
    }
}
