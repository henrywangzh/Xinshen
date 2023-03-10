using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterimage : MonoBehaviour
{
    Animator anim;
    [SerializeField] int comboNum;
    [SerializeField] Transform player;
    Vector3 position;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        StartCoroutine(Fade());
        position = player.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = position;
    }

    IEnumerator Fade()
    {
        yield return new WaitForSeconds(0.06f);
        anim.Play("FrustrationAtk" + comboNum);
        yield return new WaitForSeconds(1.2f);
        gameObject.SetActive(false);
    }


    // The following functions are placed as dummy events for the animator, and are purely used to avoid errors.
    public void ReadyCombo() {}

    public void StartSwing() {}

    public void EndSwing() {}

    public void SetFwdVelocityFrust(float vel) {}

    public void EndAttack() {}

    public void OrientTowardsInput() {}
}
