using System.Collections.Generic;
using UnityEngine;


public class FrustrationScriptController : ScriptController
{
    StancesScriptController masterController;
    Animator anim;
    PlayerAnimHandler animHandler;

    FrustrationMove move;
    FrustrationEvade evade;
    FrustrationAttack attack;
    FrustrationDashStrike dashstrike;
    AbilitiesScriptController ability;

    private void Awake()
    {
        masterController = GetComponent<StancesScriptController>();
        anim = GetComponent<Animator>();
        animHandler = GetComponent<PlayerAnimHandler>();

        // Initialize the state machine
        createStates();

        // Get references to sub-scripts
        move = GetComponent<FrustrationMove>();
        evade = GetComponent<FrustrationEvade>();
        attack = GetComponent<FrustrationAttack>();
        dashstrike = GetComponent<FrustrationDashStrike>();
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

        stateName = "dashstrike";
        state = new ScriptKeyPair(dashstrike, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node dashStrikeNode = addNode(stateName, state, null, requiredStates, canBeInterruptedByTheseStates);

        stateName = "ability";
        state = new ScriptKeyPair(ability, KeyCode.None);  // Script maps to a key. If the Keycode is None then the script cannot be switched into via keybinds
        requiredStates = new List<MonoBehaviour>();
        canBeInterruptedByTheseStates = new List<MonoBehaviour>();
        Node abilityNode = addNode(stateName, state, null, requiredStates, canBeInterruptedByTheseStates);

        moveNode.addNextAvailableStates(evadeNode);
        moveNode.addNextAvailableStates(attackNode);
        moveNode.addNextAvailableStates(moveNode);
        moveNode.addNextAvailableStates(dashStrikeNode);
        moveNode.addNextAvailableStates(abilityNode);
        evadeNode.addNextAvailableStates(moveNode);
        attackNode.addNextAvailableStates(moveNode);
        dashStrikeNode.addNextAvailableStates(moveNode);
        dashStrikeNode.addNextAvailableStates(dashStrikeNode);
        abilityNode.addNextAvailableStates(moveNode);

        setDefaultState(dashStrikeNode);
        PlayerHP.PlayerHit.AddListener(OnPlayerHit);
    }

    void OnPlayerHit()
    {
        if (!this.isActiveAndEnabled)
            return;
        GlobalVariableManager.AddStanceMeter(StancesScriptController.Stance.determination, 17);
        GlobalVariableManager.SetStanceMeter(StancesScriptController.Stance.flow, 0);
        if (GlobalVariableManager.CanTransitionStance(StancesScriptController.Stance.determination))
        {
            masterController.switchState.Invoke("determination");
        }
    }

    public void CheckStanceTransitions()
    {
        if (GlobalVariableManager.CanTransitionStance(StancesScriptController.Stance.flow))
        {
            masterController.switchState.Invoke("flow");
        }
    }

    private void OnEnable()
    {
        anim.Play("Unsheathe");
        GlobalVariableManager.Stance = StancesScriptController.Stance.frustration;
        GlobalVariableManager.Ability1 = AbilitiesScriptController.Ability.JumpSlam;
        GlobalVariableManager.ResetStanceMeters();
        animHandler.weapon.ToggleWeapon(StancesScriptController.Stance.frustration);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftBracket))
        {
            masterController.switchState.Invoke("determination");
        }
        if (Time.time % 1f <= Time.deltaTime)
        {
            GlobalVariableManager.AddStanceMeter(StancesScriptController.Stance.discord, 5);
            if (GlobalVariableManager.CanTransitionStance(StancesScriptController.Stance.discord))
            {
                masterController.switchState.Invoke("discord");
            }
        }
    }
}
