using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // A boolean to check if the item can be interacted with 
    public bool isInteractable = false;
    
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
            Debug.Log("Interact");
            // Interact
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = true;
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
