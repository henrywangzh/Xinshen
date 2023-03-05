using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] Transform trfm;
    [SerializeField] Vector3 scaleRate;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        trfm.forward = CameraController.s_cameraTrfm.position - trfm.position;
        trfm.localScale -= scaleRate;
        if (Mathf.Abs(trfm.localScale.x) < Mathf.Abs(scaleRate.x-.000001f))
        {
            Destroy(gameObject);
        }
    }
}
