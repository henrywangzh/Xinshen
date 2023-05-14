using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCorpse : MonoBehaviour
{
    [SerializeField] int numCorpses = 200;
    [SerializeField] GameObject corpse;

    static DropCorpse self;
    void Start(){
        GameObject obj = gameObject;
        // Get the room transform, size, and position
        Transform trfm = obj.transform;
        Vector3 size = trfm.localScale;
        Vector3 pos = trfm.position;
        float room_size = size.x;
        float x = pos.x, z = pos.z;
        float y = pos.y + size.y + 30;
        self = GetComponent<DropCorpse>();

        GameObject parentObject = new GameObject("Corpses");
        self.StartCoroutine(self.spawnCorpses(x, y, z, room_size/2));
    }
    
    IEnumerator spawnCorpses(float x, float y, float z, float room_size){
        float waitTime = 2.4f;
        for (int i = 0; i < numCorpses; i++){
            if (waitTime > 0.3f){
                waitTime -= 0.3f;
            }
            else {
                waitTime = Random.Range(0f, 0.1f);
            }
            Vector3 spawnPoint = GetRandomPosition(room_size);
            spawnPoint.x += x;
            spawnPoint.y = y;
            spawnPoint.z += z;
            // get random rotation
            Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            Instantiate(corpse, spawnPoint, rotation);
            yield return new WaitForSeconds(waitTime);
        }
    }

    Vector3 GetRandomPosition(float size, float minDistance = 1)
    {
        GameObject[] objs = FindObjectsOfType<GameObject>();
        // Vector3 position = new Vector3(Random.Range(-size / 2, size / 2), 0, Random.Range(-size / 2, size / 2));
        Vector3 position = ArcTanRandom(size);
        return position;
    }

    Vector3 ArcTanRandom(float size = 1){
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = size / 2f * Mathf.Sqrt(Random.Range(0f, 1f));
        Vector3 position = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
        return position;
    }

}


