using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelDungeon : MonoBehaviour
{
    // Start is called before the first frame update
    // [Header("Tutorial Room")]
    // [SerializeField] GameObject TutorialRoom;
    // [Header("Enemy Room 1")]
    // [SerializeField] GameObject EnemyRoom1;
    // [Header("Barrel Catcher Room")]
    // [SerializeField] GameObject BarrelCatcherRoom;
    // [Header("Enemy Room 2")]
    // [SerializeField] GameObject EnemyRoom2;
    // [Header("Enemy Room 3")]
    // [SerializeField] GameObject EnemyRoom3;
    [Header("Rooms (Make sure they are in order)")]
    [SerializeField] GameObject[] rooms;

    [Header("Bridges (Make sure they are in order)")]
    [SerializeField] GameObject[] bridges;

    int currentRoom;


    void Start()
    {
        for (int i = 0; i < bridges.Length; i++)
        {
            bridges[i].SetActive(false);
        }
        currentRoom = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
