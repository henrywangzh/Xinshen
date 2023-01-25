using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GlobalVariableManager : MonoBehaviour
{
    // Health Stats
    private int _health;
    private int _maxHealth;
    private bool _adrenaline;

    // Movement Stats
    private float _walkingSpeed;
    private float _runningSpeed;
    private float _jumpHeight;

    // Combat Stats
    private int _damage;
    private int _armor;
    
    // Inventory
    // private List<Item> items = new List<Item>();
    // Requires the creation of an Item class and will store objects of that class in a List
    
    //Accessors
    public int GetHealth()
    {
        return _health;
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public bool GetAdrenaline()
    {
        return _adrenaline;
    }

    public float GetWalkingSpeed()
    {
        return _walkingSpeed;
    }

    public float GetRunningSpeed()
    {
        return _runningSpeed;
    }

    public float GetJumpHeight()
    {
        return _jumpHeight;
    }

    public int GetDamage()
    {
        return _damage;
    }

    public int GetArmor()
    {
        return _armor;
    }

    /*
    public List GetInventory()
    {
        return items;
    }
    */

    // TakeDamage is called whenever damage is taken, subtracting dmg from health
    public void TakeDamage(int dmg)
    {
        _health = _health - dmg;
    }

    // Heal is called whenever the character is healed, adding amt to health
    public void Heal(int amt)
    {
        _health = _health + amt;
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
