using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This class manages all equippable abilities for the player, enabling only the active ability script at the moment and disabling
/// it upon receiving the finishAbility event. This should be linked as a node in all 4 stance ScriptControllers, and will invoke
/// the "move" node in every controller.
/// </summary>
public class AbilitiesScriptController : MonoBehaviour
{
    public UnityEvent finishAbility;
    Animator anim;
    Ability activeAbility;
    Dictionary<Ability, MonoBehaviour> abilityDict;

    FlowScriptController flowCon;
    FrustrationScriptController frustCon;
    DeterminationScriptController detCon;
    // Add discord when it's properly named

    // (STEP 1) Add a reference name for your ability in this enum
    public enum Ability
    {
        Null,  // Default state, do not assign anything to this as it is meant to be null!
        CrossSlash,
        DoubleKick,
        JumpSlam, 
        // Your ability name goes here
    }

    // (STEP 2) Add a script reference for your ability (make sure your script is DISABLED on the Player GameObject!)
    FlowCrossSlash crossSlash;
    JumpSlam jumpSlam;

    void Awake()
    {
        Setup();

        // (STEP 3) Get the reference to your ability script
        crossSlash = GetComponent<FlowCrossSlash>();
        jumpSlam = GetComponent<JumpSlam>();

        // (STEP 4) Add your script reference to this dictionary, following the format {Ability, Script}
        abilityDict = new Dictionary<Ability, MonoBehaviour>()
        {
            {Ability.CrossSlash, crossSlash},{Ability.JumpSlam, jumpSlam}, 
        };
    }

    #region Internals
    void Setup()
    {
        flowCon = GetComponent<FlowScriptController>();
        frustCon = GetComponent<FrustrationScriptController>();
        detCon = GetComponent<DeterminationScriptController>();
        anim = GetComponent<Animator>();
        finishAbility.AddListener(OnAbilityFinish);
    }
    
    void ActivateAbility(Ability name)
    {
        abilityDict[name].enabled = true;
    }

    void DeactivateAbility(Ability name)
    {
        abilityDict[name].enabled = false;
    }

    private void OnEnable()
    {
        if (Input.GetKey(KeyCode.R))  // Temp ability 1 key
        {
            if (GlobalVariableManager.Ability1 != Ability.Null)
            {
                activeAbility = GlobalVariableManager.Ability1;
                ActivateAbility(GlobalVariableManager.Ability1);
            } else
            {
                Debug.Log("Ability1 is empty in global variable manager!");
                OnAbilityFinish();
            }
        } else if (Input.GetKey(KeyCode.F))  // Temp ability 2 key
        {
            if (GlobalVariableManager.Ability2 != Ability.Null)
            {
                activeAbility = GlobalVariableManager.Ability2;
                ActivateAbility(GlobalVariableManager.Ability2);
            }
            else
            {
                Debug.LogError("Ability2 is empty in global variable manager!");
                OnAbilityFinish();
            }
        } else
        {
            OnAbilityFinish();
        }
    }

    void OnAbilityFinish()
    {

        switch (GlobalVariableManager.Stance)
        {
            case StancesScriptController.Stance.discord:
                Debug.LogWarning("Have yet to integrate discord!");
                break;
            case StancesScriptController.Stance.determination:
                Debug.LogWarning("Have yet to integrate determination!");
                break;
            case StancesScriptController.Stance.frustration:
                frustCon.switchState.Invoke("move");
                break;
            case StancesScriptController.Stance.flow:
                flowCon.switchState.Invoke("move");
                break;
            default:
                Debug.LogError("Ability manager could not read GlobalVariableManager.Stance!");
                break;
        }
    }

    private void OnDisable()
    {
        Cleanup();
    }

    // To be called the moment when disabled, to properly handle interrupted behavior
    void Cleanup()
    {
        DeactivateAbility(activeAbility);
        activeAbility = Ability.Null;

        switch (GlobalVariableManager.Stance)
        {
            case StancesScriptController.Stance.discord:
                Debug.LogWarning("Have yet to integrate discord!");
                break;
            case StancesScriptController.Stance.determination:
                Debug.LogWarning("Have yet to integrate determination!");
                break;
            case StancesScriptController.Stance.frustration:
                anim.Play("FrustrationMove");
                break;
            case StancesScriptController.Stance.flow:
                anim.Play("FlowMove");
                break;
            default:
                Debug.LogError("Ability manager could not read GlobalVariableManager.Stance!");
                break;
        }
    }

    #endregion

}
