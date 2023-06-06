using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Shatter : Enemy
{
    [SerializeField] GameObject shatteredVersion;
    [SerializeField] GameObject parent;

    [SerializeField] float power = 500;

    [SerializeField]
    private float PieceSleepCheckDelay = 0.1f;

    [SerializeField]
    private float PieceDestroyDelay = 5f;

    [SerializeField]
    private float PieceFadeSpeed = 0.25f;

    [SerializeField]
    int loadLevel = -1;

    private GameObject shatteredObject;


    public override void Die()
    {
        Destroy(GetComponent<Rigidbody>());
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;

        shatteredObject = Instantiate(shatteredVersion, transform.position, Quaternion.identity);

        Rigidbody[] rigidbodies = shatteredObject.GetComponentsInChildren<Rigidbody>();
        foreach(Rigidbody rb in rigidbodies)
        {
                rb.AddExplosionForce(power * UnityEngine.Random.Range(0.5f, 1.5f), transform.position, 10);
        }
        StartCoroutine(FadeOutRigidBodies(rigidbodies));
        if (loadLevel != -1)
        {
            SceneManager.LoadScene(loadLevel);
        }
    }

    private IEnumerator FadeOutRigidBodies(Rigidbody[] Rigidbodies)
    {
        WaitForSeconds Wait = new WaitForSeconds(PieceSleepCheckDelay);
        float activeRigidbodies = Rigidbodies.Length;

        while (activeRigidbodies > 0)
        {
            yield return Wait;

            foreach (Rigidbody rigidbody in Rigidbodies)
            {
                if (rigidbody.IsSleeping())
                {
                    activeRigidbodies--;
                }
            }
        }


        yield return new WaitForSeconds(PieceDestroyDelay);

        float time = 0;
        Renderer[] renderers = Array.ConvertAll(Rigidbodies, GetRendererFromRigidbody);

        foreach (Rigidbody body in Rigidbodies)
        {
            Destroy(body.GetComponent<Collider>());
            Destroy(body);
        }

        while (time < 1)
        {
            float step = Time.deltaTime * PieceFadeSpeed;
            foreach (Renderer renderer in renderers)
            {
                renderer.transform.Translate(Vector3.down * (step / renderer.bounds.size.y), Space.World);
            }

            time += step;
            yield return null;
        }

        foreach (Renderer renderer in renderers)
        {
            Destroy(renderer.gameObject);
        }
        Destroy(shatteredObject);
        Destroy(parent);
    }

    private Renderer GetRendererFromRigidbody(Rigidbody Rigidbody)
    {
        return Rigidbody.GetComponent<Renderer>();
    }
}
