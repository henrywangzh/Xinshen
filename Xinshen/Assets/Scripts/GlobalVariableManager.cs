using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVariableManager : MonoBehaviour {

    // Position Stats
    public static Transform PlayerSpawn;

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
    public static bool FrenzyMode;
    public static StancesScriptController.Stance Stance;
    public static Transform LockedTarget = null;
    public static AbilitiesScriptController.Ability Ability1 = AbilitiesScriptController.Ability.Null;
    public static AbilitiesScriptController.Ability Ability2 = AbilitiesScriptController.Ability.Null;

    // Frenzy Stats 
    public static int FrenzyThreshold;
    public static int FrenzyDamageBonus;
    public static int FrenzyArmorBonus;
    public static int FrenzyWalkSpeedBonus;
    public static int FrenzyRunSpeedBonus;
    /* We can add more stats later */
    
    // Screens and UI
    public static Image FrenzySplatter;
    public static Image DeathScreen;

    // Inventory
    public static List<Item> Items = new List<Item>();

    // TakeDamage is called whenever damage is taken, subtracting dmg from health
    /* IMPORTANT: this method is DEPRECATED, use PlayerHP.TakeDamage instead */
    private void TakeDamage(int dmg) 
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

    public static void SetLockedTarget(Transform targetTrfm) //call SetLockedTarget(null) to clear target lock
    {
        LockedTarget = targetTrfm;
    }
}
