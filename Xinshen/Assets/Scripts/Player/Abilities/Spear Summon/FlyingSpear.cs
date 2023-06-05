using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingSpear : MonoBehaviour
{
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * -1;
    }
}
