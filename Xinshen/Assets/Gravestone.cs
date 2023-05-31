using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravestone : Interactable
{
    [SerializeField] GameObject spirit;
    enum states {Initial, SummonedSpirit, DoingQuest, FinishedQuest };
    states currentState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    override protected void Interact()
    {
        if(currentState == states.Initial)
        {
            GameObject spiritObject = Instantiate(spirit, transform.position, Quaternion.identity);
            spiritObject.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            currentState = states.SummonedSpirit;
        }
    }
}
