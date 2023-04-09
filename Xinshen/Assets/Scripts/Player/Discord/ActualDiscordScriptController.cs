using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualDiscordScriptController : ScriptController
{
    StancesScriptController masterController;
    Animator anim;

    // Define sub-script references
    DiscordMove discordMove;
    DiscordAttack discordAttack;

    private void Awake()
    {
        masterController = GetComponent<StancesScriptController>();
        anim = GetComponent<Animator>();

        // Initialize the state machine
        createStates();

        // Get references to sub-scripts
        //move = GetComponent<FlowMove>();
        //evade = GetComponent<FlowEvade>();
        //attack = GetComponent<FlowAttack>();

        discordMove = GetComponent<DiscordMove>();
        discordAttack = GetComponent<DiscordAttack>();

        // Setting up move node
        string stateName = "discordMove";
        ScriptKeyPair state = new ScriptKeyPair(discordMove, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        List<MonoBehaviour> requiredStates = new List<MonoBehaviour>();
        List<MonoBehaviour> canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node moveNode = addNode(stateName, state, null, requiredStates, canBeInterruptedByTheseStates);

        // Setting up evade node
/*        string name = "evade";
        state = new ScriptKeyPair(evade, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node evadeNode = addNode(name, state, null, requiredStates, canBeInterruptedByTheseStates);*/

        string atkname = "discordAttack";
        state = new ScriptKeyPair(discordAttack, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node attackNode = addNode(atkname, state, null, requiredStates, canBeInterruptedByTheseStates);

        //Make a available 
        //moveNode.addNextAvailableStates(evadeNode);
        moveNode.addNextAvailableStates(attackNode);
        moveNode.addNextAvailableStates(moveNode);
        //evadeNode.addNextAvailableStates(moveNode);
        attackNode.addNextAvailableStates(moveNode);

        setDefaultState(moveNode);
    }

    private void OnEnable()
    {
        anim.Play("DiscordTransition");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            masterController.switchState.Invoke("frustration");
        }
    }
}
