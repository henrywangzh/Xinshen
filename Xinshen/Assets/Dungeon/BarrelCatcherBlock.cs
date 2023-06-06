using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCatcherBlock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string barrelTag="barrel";
    [SerializeField] int targetNumBarrels = 1;
    
    float x, y, z;
    float room_x, room_z;
    public bool activated;
    int numBarrels = 0;

    void Start()
    {
        GameObject obj = gameObject;
        // Get the room transform, size, and position
        Transform trfm = obj.transform;
        Vector3 size = trfm.localScale;
        Vector3 pos = trfm.position;
        room_x = size.x;
        room_z = size.z;
        x = pos.x;
        z = pos.z;
        y = pos.y + size.y;
        activated = false;
    }

    // Update is called once per frame
    void Update()
    {
        // count number of barrels that lies within the room
        if (numBarrels == targetNumBarrels)
            activated = true;
        else activated = false;
        Debug.Log("numBarrels: "+numBarrels);
    }
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision: "+other.gameObject.tag);
        if (other.gameObject.tag == barrelTag)
        {
            numBarrels++;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == barrelTag)
        {
            numBarrels--;
        }
    }
}
