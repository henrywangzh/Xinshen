using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordFragController : MonoBehaviour
{
    [SerializeField] GameObject[] bladeFragments;
    Rigidbody[] bladerbs;
    [SerializeField] Transform[] bladePivots;
    [SerializeField] Vector3[] guardDirections;
    [SerializeField] float guardRadius = 1f;
    [SerializeField] float guardRotationVel = 1f;
    [SerializeField] bool guarding = false;
    [SerializeField] Transform guardingTarget;
    bool prevGuarding = false;
    float baseRotSpd;

    // Start is called before the first frame update
    void Start()
    {
        if (bladeFragments.Length != bladePivots.Length)
        {
            Debug.LogError("Size mismatch of blade fragments to pivots! Blade fragments could be mapped to incorrect positions!");
        }

        bladerbs = new Rigidbody[bladeFragments.Length];
        for (int i = 0; i < bladeFragments.Length; ++i)
        {
            bladerbs[i] = bladeFragments[i].GetComponent<Rigidbody>();
        }
        baseRotSpd = guardRotationVel;
    }

    // Update is called once per frame
    void Update()
    {
        if (!guarding)
        {
            if (prevGuarding != guarding)
            {
                guardRotationVel = baseRotSpd;
            }
            for (int i = 0; i < bladeFragments.Length; ++i)
            {
                bladeFragments[i].transform.position = Vector3.Lerp(bladeFragments[i].transform.position, bladePivots[i].position, 0.2f);
                bladeFragments[i].transform.rotation = Quaternion.Lerp(bladeFragments[i].transform.rotation, bladePivots[i].rotation, 0.1f);

                /*
                Vector3 position = bladeFragments[i].transform.position;
                Vector3 dir = (bladePivots[i].position - position).normalized;
                float dist = Vector3.Distance(position, bladePivots[i].position);

                
                if (dist > 0.4f)
                {
                    bladeFragments[i].transform.position = bladePivots[i].position + dir * 0.4f;
                    bladerbs[i].velocity = Vector2.zero;
                }
                // bladerbs[i].AddForce(dir * Mathf.Pow(dist,2) * Random.Range(0.5f, 1f), ForceMode.Force);
                bladerbs[i].velocity = dir * dist * Random.Range(0.5f, 1f);
                */
                
            }
        } else
        {
            if (prevGuarding != guarding)
            {
                Debug.Log("trigger");
                guardRotationVel *= 5f;
            }
            for (int i = 0; i < bladeFragments.Length; ++i)
            {
                
                // Adjust rotation upright
                bladeFragments[i].transform.LookAt(guardingTarget, Vector3.up);

                // Adjust position to orbit around player
                Vector3 posOffset = guardDirections[i].normalized * guardRadius;
                bladeFragments[i].transform.position = Vector3.Lerp(bladeFragments[i].transform.position, guardingTarget.position + posOffset, 0.1f);

                // Rotate the blades' positions
                int nexti = i + 1 == bladeFragments.Length ? 0 : i + 1;
                guardDirections[i] = Vector3.RotateTowards(guardDirections[i], guardDirections[nexti], guardRotationVel * Time.deltaTime, 0);
                
            }
            guardRotationVel = Mathf.Lerp(guardRotationVel, baseRotSpd, 0.9f * Time.deltaTime);
        }
        prevGuarding = guarding;
    }

    public void ToggleGuard(bool guard)
    {
        guarding = guard;
    }
}
