using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalVariableManager : MonoBehaviour
{
    // Health Stats
    public int health;
    public int maxHealth;
    public bool adrenaline;

    // Movement Stats
    public float walkingSpeed;
    public float runningSpeed;
    public float jumpHeight;

    // Combat Stats
    public int damage;
    public int armor;
    
    // Inventory
    public List<Item> items = new List<Item>();

    // TakeDamage is called whenever damage is taken, subtracting dmg from health
    public void TakeDamage(int dmg)
    {
        health = health - dmg;
    }

    // Heal is called whenever the character is healed, adding amt to health
    public void Heal(int amt)
    {
        health = health + amt;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
