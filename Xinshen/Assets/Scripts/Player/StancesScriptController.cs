using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StancesScriptController : ScriptController
{
    FrustrationScriptController frustration;
    FlowScriptController flow;
    DeterminationScriptController determination;
    ActualDiscordScriptController discord;

    [SerializeField] int flowMeter;
    [SerializeField] int frustMeter;
    [SerializeField] int detMeter;
    [SerializeField] int discMeter;

    public enum Stance
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
        determination = GetComponent<DeterminationScriptController>();
        discord = GetComponent<ActualDiscordScriptController>();

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

		// Determination node
        string determinationStateName = "determination";
         state = new ScriptKeyPair(determination, KeyCode.None);
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node determinationNode = addNode(determinationStateName, state, null, requiredStates, canBeInterruptedByTheseStates);

        //discord 
        string discordName = "discord";
        state = new ScriptKeyPair(discord, KeyCode.None);
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node discordNode = addNode(discordName, state, null, requiredStates, canBeInterruptedByTheseStates);

        flowNode.addNextAvailableStates(frustNode);
        flowNode.addNextAvailableStates(discordNode);
        flowNode.addNextAvailableStates(determinationNode);
        frustNode.addNextAvailableStates(flowNode);
        frustNode.addNextAvailableStates(determinationNode);
        frustNode.addNextAvailableStates(discordNode);
        determinationNode.addNextAvailableStates(flowNode);
        determinationNode.addNextAvailableStates(frustNode);
        determinationNode.addNextAvailableStates(discordNode);
        discordNode.addNextAvailableStates(flowNode);
        discordNode.addNextAvailableStates(frustNode);
        discordNode.addNextAvailableStates(determinationNode);

        switch (defaultStance)
        {
            case Stance.discord:
                setDefaultState(discordNode);
                break;
            case Stance.determination:
                setDefaultState(determinationNode);
                break;
            case Stance.frustration:
                setDefaultState(frustNode);
                break;
            case Stance.flow:
                setDefaultState(flowNode);
                break;
        }
    }

    public void SwitchStance(Stance stance)
    {
        string stateId = "";
        switch (stance)
        {
            case Stance.discord:
                stateId = "discord";
                break;
            case Stance.determination:
                stateId = "determination";
                break;
            case Stance.flow:
                stateId = "flow";
                break;
            case Stance.frustration:
                stateId = "frustration";
                break;
        }
        switchState.Invoke(stateId);
    }

    private void Update()
    {
        flowMeter = GlobalVariableManager._flowMeter;
        frustMeter = GlobalVariableManager._frustrationMeter;
        detMeter = GlobalVariableManager._determinationMeter;
        discMeter = GlobalVariableManager._discordMeter;
    }
}
