using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StancesScriptController : ScriptController
{
    FrustrationScriptController frustration;
    FlowScriptController flow;

    enum Stance
    {
        discord,
        determination,
        frustration,
        flow,
    }

    [SerializeField] Stance defaultStance = Stance.flow;

    private void Awake()
    {
        // Initialize the state machine
        createStates();

        // Get references to sub-scripts
        frustration = GetComponent<FrustrationScriptController>();
        flow = GetComponent<FlowScriptController>(); 

        // Setting up move node
        string stateName = "frustration";
        ScriptKeyPair state = new ScriptKeyPair(frustration, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        List<MonoBehaviour> requiredStates = new List<MonoBehaviour>();
        List<MonoBehaviour> canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node frustNode = addNode(stateName, state, null, requiredStates, canBeInterruptedByTheseStates);

        // Setting up evade node
        // string name = "evade";
        // state = new ScriptKeyPair(evade, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        // requiredStates = new List<MonoBehaviour>();
        // canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        // Node evadeNode = addNode(name, state, null, requiredStates, canBeInterruptedByTheseStates);
        //
        string atkname = "flow";
        state = new ScriptKeyPair(flow, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node flowNode = addNode(atkname, state, null, requiredStates, canBeInterruptedByTheseStates);

        // moveNode.addNextAvailableStates(evadeNode);
        flowNode.addNextAvailableStates(frustNode);
        frustNode.addNextAvailableStates(flowNode);

        switch (defaultStance)
        {
            case Stance.discord:
                break;
            case Stance.determination:
                break;
            case Stance.frustration:
                setDefaultState(frustNode);
                break;
            case Stance.flow:
                setDefaultState(flowNode);
                break;
        }
    }
}
