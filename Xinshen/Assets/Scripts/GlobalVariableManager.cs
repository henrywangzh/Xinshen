using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GlobalVariableManager : MonoBehaviour {

    // Position Stats
    public static Transform PlayerSpawn;
    public static bool OnGround;

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
    private static int _discordMeter = 0;
    private static int _determinationMeter = 0;
    private static int _frustrationMeter = 0;
    private static int _flowMeter = 0;

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

    // Camera
    public static Transform MainCamera;

    // TakeDamage is called whenever damage is taken, subtracting dmg from health
    /* IMPORTANT: this method is DEPRECATED, use PlayerHP.TakeDamage instead */
    private void TakeDamage(int dmg) 
    {
        Debug.Log("Taking damage");
        Health = Health - dmg;
    }

    // Heal is called whenever the character is healed, adding amt to health such that it doesn't go over MaxHealth
    // NOTE: This method is also deprecated. Refer to PlayerHP class instead
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

    #region Stance Switching

    public static void AddStanceMeter(StancesScriptController.Stance stance, int amount)
    {
        switch (stance)
        {
            case StancesScriptController.Stance.discord:
                _discordMeter += amount;
                if (_discordMeter > 100)
                    _discordMeter = 100;
                break;
            case StancesScriptController.Stance.determination:
                _determinationMeter += amount;
                if (_determinationMeter > 100)
                    _determinationMeter = 100;
                break;
            case StancesScriptController.Stance.flow:
                _flowMeter += amount;
                if (_flowMeter > 100)
                    _flowMeter = 100;
                break;
            case StancesScriptController.Stance.frustration:
                _frustrationMeter += amount;
                if (_frustrationMeter > 100)
                    _frustrationMeter = 100;
                break;
        }
    }

    public static bool CanTransitionStance(StancesScriptController.Stance stance)
    {
        switch (stance)
        {
            case StancesScriptController.Stance.discord:
                return _discordMeter >= 100;
            case StancesScriptController.Stance.determination:
                return _determinationMeter >= 100;
            case StancesScriptController.Stance.flow:
                return _flowMeter >= 100;
            case StancesScriptController.Stance.frustration:
                return _frustrationMeter >= 100;
        }
        return false;
    }

    #endregion

    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    {
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            OnGround |= normal.y >= 0.6f;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        OnGround = false;
    }

}
