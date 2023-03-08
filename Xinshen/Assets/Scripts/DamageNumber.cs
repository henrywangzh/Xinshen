using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] Transform trfm;
    [SerializeField] Rigidbody rb;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Vector3 scaleRate, initVelocity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Init(int value, Vector3 target)
    {
        GameManager.SetCanvasChild(trfm);
        trfm.position = target;

        text.text = value.ToString();

        trfm.forward = CameraController.s_cameraTrfm.position - trfm.position;
        rb.velocity = trfm.up * initVelocity.y + trfm.right * Random.Range(-initVelocity.x, initVelocity.x);
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
