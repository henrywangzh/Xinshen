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
    bool[] spawnPointUsed;
    
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
        while (currentDifficulty < targetDifficulty)
        {
            spawnIndex = Random.Range(0, spawnPoints.Count);
            while (spawnPointUsed[spawnIndex]) { spawnIndex = Random.Range(0, spawnPoints.Count); }

            enemyIndex = Random.Range(0, enemies.Length);
        }
    }
}
