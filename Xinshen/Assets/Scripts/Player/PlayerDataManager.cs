using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager instance;

    private static Dictionary<string, int> tombstoneInteracts;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            tombstoneInteracts = new Dictionary<string, int>();
        }
    }

    public static void LogInteraction(string name)
    {
        if (tombstoneInteracts.ContainsKey(name))
        {
            tombstoneInteracts[name]++;
        } else
        {
            tombstoneInteracts[name] = 1;
        }
    }
    
    public static int GetInteraction(string name)
    {
        if (tombstoneInteracts.ContainsKey(name))
        {
            return tombstoneInteracts[name];
        }
        else
        {
            return 0;
        }
    }
}
