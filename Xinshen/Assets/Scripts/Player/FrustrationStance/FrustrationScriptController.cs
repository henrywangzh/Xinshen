using System.Collections.Generic;
using UnityEngine;


public class FrustrationScriptController : ScriptController
{
    StancesScriptController masterController;
    Animator anim;

    FrustrationMove move;
    // FlowEvade evade;
    FrustrationAttack attack;
    FrustrationDashStrike dashstrike;

    private void Awake()
    {
        masterController = GetComponent<StancesScriptController>();
        anim = GetComponent<Animator>();

        // Initialize the state machine
        createStates();

        // Get references to sub-scripts
        move = GetComponent<FrustrationMove>();
        // evade = GetComponent<FlowEvade>();
        attack = GetComponent<FrustrationAttack>();
        dashstrike = GetComponent<FrustrationDashStrike>();

        // Setting up move node
        string stateName = "move";
        ScriptKeyPair state = new ScriptKeyPair(move, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        List<MonoBehaviour> requiredStates = new List<MonoBehaviour>();
        List<MonoBehaviour> canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node moveNode = addNode(stateName, state, null, requiredStates, canBeInterruptedByTheseStates);

        // Setting up evade node
        // string name = "evade";
        // state = new ScriptKeyPair(evade, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        // requiredStates = new List<MonoBehaviour>();
        // canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        // Node evadeNode = addNode(name, state, null, requiredStates, canBeInterruptedByTheseStates);
        //
        string atkname = "attack";
        state = new ScriptKeyPair(attack, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node attackNode = addNode(atkname, state, null, requiredStates, canBeInterruptedByTheseStates);

        stateName = "dashstrike";
        state = new ScriptKeyPair(dashstrike, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node dashStrikeNode = addNode(stateName, state, null, requiredStates, canBeInterruptedByTheseStates);

        // moveNode.addNextAvailableStates(evadeNode);
        moveNode.addNextAvailableStates(attackNode);
        moveNode.addNextAvailableStates(moveNode);
        moveNode.addNextAvailableStates(dashStrikeNode);
        // evadeNode.addNextAvailableStates(moveNode);
        attackNode.addNextAvailableStates(moveNode);
        dashStrikeNode.addNextAvailableStates(moveNode);
        dashStrikeNode.addNextAvailableStates(dashStrikeNode);

        setDefaultState(dashStrikeNode);
    }

    private void OnEnable()
    {
        anim.Play("Unsheathe");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            masterController.switchState.Invoke("flow");
        }
    }
}
