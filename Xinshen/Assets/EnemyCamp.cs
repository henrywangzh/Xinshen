using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamp : MonoBehaviour
{

    [SerializeField] List<GameObject> enemies;
    [SerializeField] GameObject chest;
    [SerializeField] List<Transform> spawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        foreach(GameObject enemy in enemies)
        {
            GameObject enemyInstance = Instantiate(enemy, spawnPoints[Random.Range(0, spawnPoints.Count)]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
