using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalVariableManager : MonoBehaviour {

    // Health Stats
    public static int Health;
    public static int MaxHealth;
    public static bool Adrenaline;

    // Movement Stats
    public static float WalkingSpeed;
    public static float RunningSpeed;
    public static float JumpHeight;

    // Combat Stats
    public static int Damage;
    public static int Armor;
    public static string Stance;
    
    // Inventory
    public static List<Item> Items = new List<Item>();

    // TakeDamage is called whenever damage is taken, subtracting dmg from health
    public static void TakeDamage(int dmg)
    {
        Debug.Log("Taking damage");
        Health = Health - dmg;
    }

    // Heal is called whenever the character is healed, adding amt to health such that it doesn't go over MaxHealth
    public static void Heal(int amt)
    {
        Health = Health + amt;
        
        if (Health > MaxHealth)
        {
            Health = MaxHealth;
        }
    }
}
