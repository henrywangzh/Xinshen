using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowAttack : MonoBehaviour
{
    Animator anim;
    FlowScriptController controller;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<FlowScriptController>();
    }

    private void OnEnable()
    {
        if (anim == null)
            anim = GetComponent<Animator>();
        SetCombo(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SetCombo(1);
        }
    }

    public void SetCombo(int combo)
    {
        anim.SetBool("Combo", combo >= 1);
    }

    public void EndAttack()
    {
        if (this.isActiveAndEnabled)
            controller.switchState.Invoke("move"); 
    }
}
