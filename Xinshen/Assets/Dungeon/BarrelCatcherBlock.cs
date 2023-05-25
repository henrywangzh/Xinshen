using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelCatcherBlock : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] string barrelTag;
    [SerializeField] int targetNumBarrels = 1;
    
    float x, y, z;
    float room_x, room_z;
    bool activated;
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
        int numBarrels = 0;
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag(barrelTag);
        foreach (GameObject obj in allObjects)
        {
            float objX = obj.transform.position.x;
            float objZ = obj.transform.position.z;

            if (objX >= x-room_x && objX <= x+room_x && objZ >= z-room_z && objZ <= z+room_z)
            {
                numBarrels++;
            }
        }
        if (numBarrels == targetNumBarrels)
            activated = true;
        else activated = false;

        Debug.Log(numBarrels);

    }
}
