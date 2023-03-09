using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCamp : MonoBehaviour
{

    [SerializeField] List<GameObject> enemies;
    [SerializeField] GameObject chest;
    [SerializeField] List<Transform> spawnPoints;

    List<Enemy> activeEnemies;

    // Start is called before the first frame update
    void Start()
    {
        activeEnemies = new List<Enemy>();
        foreach(GameObject enemy in enemies)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            GameObject enemyInstance = Instantiate(enemy, spawnPoint);
            foreach(Enemy e in enemyInstance.GetComponentsInChildren<Enemy>())
            {
                activeEnemies.Add(e);
                e.AssignToCamp(this);
            }
            spawnPoints.Remove(spawnPoint);
        }
    }
    public void removeEnemy(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
    }
    // Update is called once per frame
    void Update()
    {
        if (activeEnemies.Count == 0)
        {
            Debug.Log("here");
            // camp has been cleared, spawn chest
            Instantiate(chest, this.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
