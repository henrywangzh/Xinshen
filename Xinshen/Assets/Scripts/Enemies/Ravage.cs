using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ravage :  Enemy
{


    [SerializeField] float attack_range = 5;
    [SerializeField] float combat_range = 10;
    [SerializeField] float detection_range = 20;
    public GameObject Player;
    public float distance;
    public bool isAngered;
    public NavMeshAgent _agent;


    public override void Die()
    {
        Debug.Log("ahhhh im dying");
        Destroy(gameObject);
    }

    
    private void double_swipe_attack()
    {
        // meed to be implemented 
    }
    private void Lunge()
    {
        int cooldown = 20;
        float timer = 0f;
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            double_swipe_attack();
            timer = cooldown;
        } 

    }
    

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(Player.transform.position, this.transform.position);
        if (distance <= combat_range)
        {
            isAngered = true; //switch on the enemy moving mode
        } 

        if(distance > detection_range)
        {
            isAngered = false; //The enemy stop chasing after the player
        }


        if (isAngered && distance >= attack_range)
        {
            _agent.isStopped = false;
            _agent.SetDestination(Player.transform.position);

        }
        else // The player is in the attack range of the enemy
        {
            _agent.isStopped = true;
            
            //need to implement the lunge action
            //call the Lunge function
        }


    }
}
