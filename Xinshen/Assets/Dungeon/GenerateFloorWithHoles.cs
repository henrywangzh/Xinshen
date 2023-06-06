using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloorWithHoles : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int roomSize = 33;
    [SerializeField] int holeOffset = 7;
    [SerializeField] int holeSize = 5;
    [SerializeField] GameObject hole=null;
    [Header("Dimension of floor should match a unit cube")]
    [SerializeField] GameObject floor=null;
    [SerializeField] GameObject bridge=null;

    GameObject[] barrelCatcherBlocks;
    bool roomClear = false;
    void Start()
    {
        if (!floor) 
            floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject obj = gameObject;
        // Get the room transform, size, and position
        Transform trfm = obj.transform;
        Vector3 size = trfm.localScale;
        Vector3 pos = trfm.position;
        float room_x = size.x;
        float room_z = size.z;
        float x = pos.x;
        float z = pos.z;
        float y = pos.y;
        float[] dirX = {-1, 1, 1, -1};
        float[] dirZ = {-1, -1, 1, 1};

        // Generate the corners
        for (int i = 0; i!=4; i++){
            GameObject corner = Instantiate(floor, trfm);
            float offset = (roomSize-holeOffset) / 2;
            corner.transform.position = new Vector3(x + dirX[i]*offset, y, z + dirZ[i]*offset);
            corner.transform.localScale = new Vector3(holeOffset, 1, holeOffset);
        }
        // Generate the connectting pieces for the corners and the hols

        float[] dirX2 = {-1, 1, 0, 0};
        float[] dirZ2 = {0, 0, -1, 1};
        float[] scaleX = {holeOffset, holeOffset, roomSize-holeOffset*2, roomSize-holeOffset*2};
        float[] scaleZ = {roomSize-2*holeOffset, roomSize-holeOffset*2, holeOffset, holeOffset};
        for (int i = 0; i!=4; i++){
            GameObject connectCorner = Instantiate(floor, trfm);

            float offset = (roomSize-holeOffset) / 2;
            connectCorner.transform.position = new Vector3(x + dirX2[i]*offset, y, z + dirZ2[i]*offset);
            connectCorner.transform.localScale = new Vector3(scaleX[i], 1, scaleZ[i]);
        }
        float[] scaleX2 = {holeSize, holeSize, roomSize-holeSize*2-holeOffset*2, roomSize-holeSize*2-holeOffset*2};
        float[] scaleZ2 = {roomSize-holeSize*2-holeOffset*2, roomSize-holeSize*2-holeOffset*2, holeSize, holeSize};
        for (int i = 0; i!=4; i++){
            GameObject connectHole = Instantiate(floor, trfm);
            float offset = (roomSize-holeSize) / 2 - holeOffset;
            connectHole.transform.position = new Vector3(x + dirX2[i]*offset, y, z + dirZ2[i]*offset);
            connectHole.transform.localScale = new Vector3(scaleX2[i], 1, scaleZ2[i]);
        }
        // Create Center
        GameObject center = Instantiate(floor, trfm);
        center.transform.position = new Vector3(x, y, z);
        center.transform.localScale = new Vector3(roomSize-holeSize*2-holeOffset*2, 1, roomSize-holeSize*2-holeOffset*2);


        // Generate the holes
        if (hole){
            barrelCatcherBlocks = new GameObject[4];
            for (int i=0; i!=4; i++){
                GameObject holeObj = Instantiate(hole, trfm);
                float offset = (roomSize-holeSize) / 2 - holeOffset;
                holeObj.transform.position = new Vector3(x + dirX[i]*offset, y, z + dirZ[i]*offset);
                holeObj.transform.localScale = new Vector3(holeSize, 1, holeSize);
                barrelCatcherBlocks[i] = holeObj;
            }
        }
        floor.SetActive(false);
        bridge.SetActive(false);
            
    }

    // Update is called once per frame
    void Update()
    {
        roomClear = true;
        for (int i = 0; i!=4; i++){
            if (!(barrelCatcherBlocks[i].GetComponentsInChildren<BarrelCatcherBlock>()[0].activated)){
                roomClear = false;
                break;
            }
        }
        bridge.SetActive(roomClear);
        // Debug.Log("roomClear: " + roomClear);
    }
}
