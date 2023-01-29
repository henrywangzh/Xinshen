using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string name;
    public string description;
    public string type;  // ie. weapon, quest items, etc.
    public Sprite icon;
    
    // Constructor
    public Item(int id, string name, string description, string type, Sprite icon)
    {
        
        this.id = id;
        this.name = name;
        this.description = description;
        this.type = type;
        // this finds the sprite for the item with the right name from the Resources/Textures/ folder
        this.icon = Resources.Load("/Sprites/" + name) as Sprite;
        
    }
}
