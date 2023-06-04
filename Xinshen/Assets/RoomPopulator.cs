using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPopulator : MonoBehaviour
{
    public int targetDifficulty, currentDifficulty;
    [SerializeField] Transform[] chunkPoints;
    [SerializeField] GameObject[] chunkObj;
    [SerializeField] List<Transform> spawnPoints;
    [SerializeField] GameObject[] enemies;
    [SerializeField] int[] enemyDifficulty;
    [SerializeField] bool[] spawnPointUsed;
    
    // Start is called before the first frame update
    void Start()
    {
        Transform[] tempTrfms;
        for (int i = 0; i < chunkPoints.Length; i++)
        {
            tempTrfms = Instantiate(chunkObj[Random.Range(0, chunkObj.Length)], chunkPoints[i].position, Quaternion.Euler(0, Random.Range(0, 4) * 90, 0)).GetComponent<Chunk>().spawnPoints;

            for (int j = 0; j < tempTrfms.Length; j++)
            {
                spawnPoints.Add(tempTrfms[j]);
            }
        }

        spawnPointUsed = new bool[spawnPoints.Count];

        int spawnIndex, enemyIndex;
        while (currentDifficulty < targetDifficulty - 5)
        {
            spawnIndex = Random.Range(0, spawnPoints.Count);

            for (int i = 0; i < 500; i++)
            {
                if (spawnPointUsed[spawnIndex]) { spawnIndex = Random.Range(0, spawnPoints.Count); }
                else { break; }
            }

            enemyIndex = Random.Range(0, enemies.Length);
            for (int i = 0; i < 500; i++)
            {
                if (currentDifficulty + enemyDifficulty[enemyIndex] > Mathf.RoundToInt(targetDifficulty * 1.3f)) { enemyIndex = Random.Range(0, enemies.Length); }
                else { break; }
            }

            Instantiate(enemies[enemyIndex], spawnPoints[spawnIndex].position, Quaternion.Euler(0, Random.Range(0, 360), 0));

            currentDifficulty += enemyDifficulty[enemyIndex];
            spawnPointUsed[spawnIndex] = true;
        }
    }
}
