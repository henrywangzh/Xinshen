using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloorContent : MonoBehaviour
{
    [SerializeField] GameObject[] enemyCamps;
    [SerializeField] Enemy[] enemies;
    [SerializeField] GameObject[] chests;
    [SerializeField] GameObject[] traps;
    [SerializeField] GameObject[] misc;
    [SerializeField] float enemyCampSpawnChance = 0.1f, enemySpawnChance = 0.5f, chestSpawnChance = 0.2f;

    void Start(){
        GameObject obj = gameObject;
        // Get the room transform, size, and position
        Transform trfm = obj.transform;
        Vector3 size = trfm.localScale;
        Vector3 pos = trfm.position;
        float room_size = size.x;
        float x = pos.x, z = pos.z;
        float y = pos.y + size.y; 

        Debug.Log("size: " + size);
        Debug.Log("pos: " + pos);

        // Generate enemies in random spawn points
        int numEnemies = (int) (room_size/4);
        for (int i = 0; i < numEnemies; i++){
            Vector3 spawnPoint = GetRandomPosition(room_size);
            spawnPoint.x += x;
            spawnPoint.y = y;
            spawnPoint.z += z;
            if (Random.Range(0f, 1f) < enemySpawnChance){
                Instantiate(enemies[Random.Range(0, enemies.Length)], spawnPoint, Quaternion.identity);
            }
            Debug.Log("spawnPoint: " + spawnPoint);
        }

        // Generate misc objects in random spawn points
        int numMisc = (int) (room_size/2);
        for (int i = 0; i < numMisc; i++){
            Vector3 spawnPoint = GetRandomPosition(room_size);
            spawnPoint.x += x;
            spawnPoint.y = y;
            spawnPoint.z += z;
            if (Random.Range(0f, 1f) < enemySpawnChance){
                Instantiate(misc[Random.Range(0, misc.Length)], spawnPoint, Quaternion.identity);
            }
            Debug.Log("spawnPoint: " + spawnPoint);
        }
    }

    // Vector3 GetRandomPosition(float size, float minDistance = 1)
    // {
    //     Enemy[] enemies = FindObjectsOfType<Enemy>();
    //     while (true)
    //     {
    //         Vector3 position = new Vector3(Random.Range(-size / 2, size / 2), 0, Random.Range(-size / 2, size / 2));

    //         bool isFarEnough = true;
    //         foreach (Enemy enemy in enemies)
    //         {
    //             if (Vector3.Distance(enemy.transform.position, position) < minDistance)
    //             {
    //                 isFarEnough = false;
    //                 break;
    //             }
    //         }

    //         if (isFarEnough)
    //         {
    //             return position;
    //         }
    //     }
    // }

    // Returns a random position in the room that is at least minDistance away from all other objects
    Vector3 GetRandomPosition(float size, float minDistance = 1)
    {
        GameObject[] objs = FindObjectsOfType<GameObject>();
        while (true)
        {
            // Vector3 position = new Vector3(Random.Range(-size / 2, size / 2), 0, Random.Range(-size / 2, size / 2));
            Vector3 position = ArcTanRandom(size);
            bool isFarEnough = true;
            foreach (GameObject obj in objs)
            {
                if (Vector3.Distance(obj.transform.position, position) < minDistance)
                {
                    isFarEnough = false;
                    break;
                }
            }

            if (isFarEnough)
            {
                return position;
            }
        }
    }

    Vector3 ArcTanRandom(float size = 1){
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float radius = size / 2f * Mathf.Sqrt(Random.Range(0f, 1f));
        Vector3 position = new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
        return position;
    }
}