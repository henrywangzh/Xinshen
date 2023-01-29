using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            float InteractRadius = 2f;
            //returns array of colliders that lie within a specified radius of the player
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, InteractRadius); 
            foreach (Collider collider in colliderArray)
            {
                Debug.Log(collider);
            }
        }
        
    }
}
