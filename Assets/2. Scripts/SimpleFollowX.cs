using System;
using System.Collections;
using UnityEngine;

public class SimpleFollowX : MonoBehaviour
{
    public GameObject target;
    public float delay = 0.5f;

    private void Update()
    {
        StartCoroutine(FollowTarget(target, delay));
    }

    private IEnumerator FollowTarget(GameObject target, float delay)
    {
        var curPos = target.transform.position.x;
        yield return new WaitForSeconds(delay);
        transform.position =  new Vector3(curPos, transform.position.y, transform.position.z);
    }
}
