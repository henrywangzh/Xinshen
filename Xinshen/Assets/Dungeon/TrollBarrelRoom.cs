using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollBarrelRoom : MonoBehaviour
{
    [SerializeField] int height;
    [SerializeField] GameObject barrel;
    [SerializeField] Vector3 barrelSize = new Vector3(1, 1, 1);


    void Start(){
        GameObject obj = gameObject;
        // Get the room transform, size, and position
        Transform trfm = obj.transform;
        Vector3 size = trfm.localScale;
        Vector3 pos = trfm.position;
        float room_size = size.x;
        float x = pos.x, z = pos.z;
        float y = pos.y + size.y;

        GameObject parentObject = new GameObject("Barrels");


        FillRoom(x - room_size/2, y, z - room_size/2, room_size, parentObject);
    }

   

    private void FillRoom(float xPos, float yPos, float zPos, float roomSize, GameObject parentObject)
    {   
        Vector3 objectSize = barrelSize;
        for (float x = objectSize.x / 2; x < roomSize; x += objectSize.x)
        {
            for (float y = objectSize.y / 2; y < height; y += objectSize.y)
            {
                for (float z = objectSize.z / 2; z < roomSize; z += objectSize.z)
                {
                    Vector3 spawnPosition = new Vector3(x+xPos, y+yPos, z+zPos);
                    Instantiate(barrel, spawnPosition, Quaternion.identity, parentObject.transform);
                }
            }
        }
    }

}


