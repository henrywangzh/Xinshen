using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashFX : MonoBehaviour
{
    [SerializeField] Vector3 initialScale, changeScale;
    [SerializeField] float backOffset, forwardRate;
    static Vector3 vect3;
    Transform trfm;

    // Start is called before the first frame update
    void Start()
    {
        trfm = transform;

        trfm.localScale = initialScale;
        trfm.forward = CameraController.s_cameraTrfm.position - trfm.position;
        trfm.Rotate(Vector3.forward * Random.Range(-20,21));
        trfm.position -= trfm.right * backOffset;
        vect3.z = 1;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vect3.x = trfm.localScale.x * changeScale.x;
        vect3.y = trfm.localScale.y + changeScale.y;

        trfm.position += trfm.right * forwardRate;

        if (Mathf.Abs(trfm.localScale.y) < Mathf.Abs(changeScale.y) - .01f)
        {
            Destroy(gameObject);
        }
        else
        {
            trfm.localScale = vect3;
        }
    }
}
