using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBomb : MonoBehaviour
{
    [SerializeField] float explodeDelay = 2f;
    SphereCollider col;
    [SerializeField] int damage = 10;
    [SerializeField] ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<SphereCollider>();
        col.enabled = false;
        StartCoroutine(StartExplosion());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            PlayerHP.TakeDamage(damage);
        }
    }

    IEnumerator StartExplosion()
    {
        yield return new WaitForSeconds(explodeDelay);
        col.enabled = true;
        ps.Play();
        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
