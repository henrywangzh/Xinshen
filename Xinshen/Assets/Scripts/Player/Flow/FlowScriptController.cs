using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowScriptController : ScriptController
{
    StancesScriptController masterController;
    Animator anim;
    PlayerAnimHandler animHandler;

    // Define sub-script references
    FlowMove move;
    FlowEvade evade;
    FlowAttack attack;
    AbilitiesScriptController ability;
    FlowCrossSlash cross;

    private void Awake()
    {
        masterController = GetComponent<StancesScriptController>();
        anim = GetComponent<Animator>();
        animHandler = GetComponent<PlayerAnimHandler>();

        // Initialize the state machine
        createStates();

        // Get references to sub-scripts
        move = GetComponent<FlowMove>();
        evade = GetComponent<FlowEvade>();
        attack = GetComponent<FlowAttack>();
        cross = GetComponent<FlowCrossSlash>();
        ability = GetComponent<AbilitiesScriptController>();

        // Setting up move node
        string stateName = "move";
        ScriptKeyPair state = new ScriptKeyPair(move, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        List<MonoBehaviour> requiredStates = new List<MonoBehaviour>();
        List<MonoBehaviour> canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node moveNode = addNode(stateName, state, null, requiredStates, canBeInterruptedByTheseStates);

        // Setting up evade node
        string name = "evade";
        state = new ScriptKeyPair(evade, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node evadeNode = addNode(name, state, null, requiredStates, canBeInterruptedByTheseStates);

        string atkname = "attack";
        state = new ScriptKeyPair(attack, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node attackNode = addNode(atkname, state, null, requiredStates, canBeInterruptedByTheseStates);

        atkname = "cross";
        state = new ScriptKeyPair(cross, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node crossNode = addNode(atkname, state, null, requiredStates, canBeInterruptedByTheseStates);

        stateName = "ability";
        state = new ScriptKeyPair(ability, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node abilityNode = addNode(stateName, state, null, requiredStates, canBeInterruptedByTheseStates);

        moveNode.addNextAvailableStates(evadeNode);
        moveNode.addNextAvailableStates(attackNode);
        moveNode.addNextAvailableStates(moveNode);
        moveNode.addNextAvailableStates(crossNode);
        moveNode.addNextAvailableStates(abilityNode);
        evadeNode.addNextAvailableStates(moveNode);
        attackNode.addNextAvailableStates(moveNode);
        attackNode.addNextAvailableStates(evadeNode);
        attackNode.addNextAvailableStates(crossNode);
        attackNode.addNextAvailableStates(abilityNode);
        crossNode.addNextAvailableStates(moveNode);
        abilityNode.addNextAvailableStates(moveNode);


        setDefaultState(moveNode);
        PlayerHP.PlayerHit.AddListener(OnPlayerHit);
    }

    void OnPlayerHit()
    {
        if (!this.isActiveAndEnabled)
            return;
        GlobalVariableManager.AddStanceMeter(StancesScriptController.Stance.discord, 34);
        if (GlobalVariableManager.CanTransitionStance(StancesScriptController.Stance.discord))
        {
            masterController.switchState.Invoke("discord");
        }
    }

    private void OnEnable()
    {
        anim.Play("FlowTransitionIn");
        GlobalVariableManager.Stance = StancesScriptController.Stance.flow;
        GlobalVariableManager.Ability1 = AbilitiesScriptController.Ability.CrossSlash;
        GlobalVariableManager.Ability2 = AbilitiesScriptController.Ability.DoubleKick;
        GlobalVariableManager.ResetStanceMeters();
        animHandler.weapon.ToggleWeapon(StancesScriptController.Stance.flow);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            masterController.switchState.Invoke("frustration");
        }
    }
}
