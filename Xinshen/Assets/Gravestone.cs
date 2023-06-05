using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class Gravestone : Interactable
{
    DialogueRunner runner;
    [SerializeField] string dialogueNode;
    [SerializeField] GameObject spirit;
    enum states {Initial, SummonedSpirit, DoingQuest, FinishedQuest };
    states currentState;
    GameObject spiritObject;

    // Start is called before the first frame update
    void Start()
    {
        runner = FindObjectOfType<DialogueRunner>();
        runner.onDialogueComplete.AddListener(ResetInteractState);
    }

    [YarnCommand("StoreInteraction")]
    public void StoreInteraction(string name)
    {
        PlayerDataManager.LogInteraction(name);
    }

    [YarnCommand("GetInteraction")]
    public void GetInteraction(string name)
    {
        PlayerDataManager.GetInteraction(name);
    }

    void ResetInteractState()
    {
        currentState = states.Initial;
        Destroy(spiritObject);
    }

    override protected void Interact()
    {
        if(currentState == states.Initial)
        {
            spiritObject = Instantiate(spirit, transform.position, Quaternion.identity);
            spiritObject.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            currentState = states.SummonedSpirit;
            runner.StartDialogue(dialogueNode);
        }
    }
}
