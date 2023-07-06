using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance; // Singleton instance

    public int gold;
    public int health;

    private void Awake()
    {
        // Singleton setup
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);

        // Init round attributes
        gold = 100;
        health = 10;
    }

    public void AddGold(int amount)
    {
        gold += amount;
    }

    public void SpendGold(int amount)
    {
        if(gold >= amount)
            gold -= amount;
        else
            Debug.Log("not enough gold");
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        //Debug.Log("Current health " + health);
        // check for death
        if(health <= 0)
        {
            Debug.Log("GAME OVER");
            Time.timeScale = 0;
        }
    }
}
