using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StanceUI : MonoBehaviour
{
    [SerializeField] float magnificationFactor = 1f;
    [SerializeField] float magnificationSpeed = 1f;
    [SerializeField] float turnSpeed = 5f;
    [SerializeField] GameObject sigil;
    [SerializeField] RectTransform determinationSlot;
    [SerializeField] RectTransform frustrationSlot;
    [SerializeField] RectTransform flowSlot;
    [SerializeField] RectTransform discordSlot;
    [SerializeField] Image determinationIcon;
    [SerializeField] Image frustrationIcon;
    [SerializeField] Image flowIcon;
    [SerializeField] Image discordIcon;
    [SerializeField] ParticleSystem LitPs;
    [SerializeField] Sprite determinationOff;
    [SerializeField] Sprite determinationOn;
    [SerializeField] Sprite frustrationOff;
    [SerializeField] Sprite frustrationOn;
    [SerializeField] Sprite flowOff;
    [SerializeField] Sprite flowOn;
    [SerializeField] Sprite discordOff;
    [SerializeField] Sprite discordOn;
    
    Transform curIcon;

    // Start is called before the first frame update
    void Start()
    {
        GlobalVariableManager.StanceChanged.AddListener(OnStanceSwitch);
    }
    
    // Update is called once per frame
    void Update()
    {
        determinationIcon.transform.position = determinationSlot.position;
        frustrationIcon.transform.position = frustrationSlot.position;
        flowIcon.transform.position = flowSlot.position;
        discordIcon.transform.position = discordSlot.position;
    }

    void OnStanceSwitch()
    {
        /*
         * When a stance switch occurs, spin the sigil to place the active stance on the bottom, if not discord.
         * Once the spin is in position, flash the icon. 
         */
        StartCoroutine(SpinUI(GlobalVariableManager.Stance));
    }

    IEnumerator SpinUI(StancesScriptController.Stance stance)
    {
        /*
         * Valid rotation vectors (transform.up): (0, -1), (1/2, rt(3)/2), (-1/2, rt(3)/2)
         */
        Vector2 orientation = Vector2.zero;

        // Set symbols to be unlit
        discordIcon.sprite = discordOff;
        determinationIcon.sprite = determinationOff;
        frustrationIcon.sprite = frustrationOff;
        flowIcon.sprite = flowOff;

        if (curIcon != null)
        {
            float targScale = curIcon.localScale.x / magnificationFactor;
            Vector3 target = new Vector3(targScale, targScale, targScale);
            while (Vector3.Distance(curIcon.localScale, target) > 0.005f)
            {
                curIcon.localScale = Vector3.MoveTowards(curIcon.localScale, target, Time.fixedDeltaTime * magnificationSpeed);
                yield return new WaitForFixedUpdate();
            }
        }

        switch (stance)
        {
            case StancesScriptController.Stance.discord:
                break;
            case StancesScriptController.Stance.determination:
                orientation = new Vector2(-1.732f / 2, 0.5f).normalized;
                break;
            case StancesScriptController.Stance.frustration:
                orientation = new Vector2(0, -1);
                break;
            case StancesScriptController.Stance.flow:
                orientation = new Vector2(1.732f / 2, 0.5f).normalized;
                break;
        }

        yield return new WaitForFixedUpdate();

        if (orientation != Vector2.zero)
        {
            while (sigil.transform.up != (Vector3)orientation)
            {
                if (Vector3.Distance(sigil.transform.up, orientation) >= 2f)  // to stamp edge cases where rotation goes out of xy plane
                {
                    sigil.transform.up = Vector3.RotateTowards(sigil.transform.up, sigil.transform.right, Time.fixedDeltaTime, 0);
                }
                sigil.transform.up = Vector3.RotateTowards(sigil.transform.up, orientation, Time.fixedDeltaTime * turnSpeed, 0);
                yield return new WaitForFixedUpdate();
            }
        }
        
        // Play VFX, Light up symbol
        switch (stance)
        {
            case StancesScriptController.Stance.discord:
                discordIcon.sprite = discordOn;
                curIcon = discordIcon.transform;
                break;
            case StancesScriptController.Stance.determination:
                determinationIcon.sprite = determinationOn;
                LitPs.Play();
                curIcon = determinationIcon.transform;
                break;
            case StancesScriptController.Stance.frustration:
                frustrationIcon.sprite = frustrationOn;
                LitPs.Play();
                curIcon = frustrationIcon.transform;
                break;
            case StancesScriptController.Stance.flow:
                flowIcon.sprite = flowOn;
                LitPs.Play();
                curIcon = flowIcon.transform;
                break;
        }

        // Expand active icon

        if (curIcon != null)
        {
            float targScale = curIcon.localScale.x * magnificationFactor;
            Vector3 target = new Vector3(targScale, targScale, targScale);
            while (Vector3.Distance(curIcon.localScale, target) > 0.005f)
            {
                curIcon.localScale = Vector3.MoveTowards(curIcon.localScale, target, Time.fixedDeltaTime * magnificationSpeed);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}