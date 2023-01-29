using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // A boolean to check if the item can be interacted with 
    public bool isInteractable = false;
    private Collider collider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If can be interacted with and player presses 'F'
        if (isInteractable && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("Interacted with " + gameObject.tag);

            if (gameObject.tag == "Campfire")
            {
                Debug.Log(collider.tag + " healed at a campfire");
                GlobalVariableManager.Heal(9999);
                
            } else if (gameObject.tag == "NPC")
            {
                Debug.Log(collider.tag + " interacted with " + gameObject.tag);
                // Interact with NPC
                
            } else if (gameObject.tag == "Sign")
            {
                Debug.Log(collider.tag + " interacted with " + gameObject.tag);
                // Interact with a sign
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = true;
            collider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = false;
        }
    }
    
    
}
