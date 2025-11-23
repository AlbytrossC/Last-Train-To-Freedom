using System;
using System.Collections;
using UnityEngine;

public class SimpleLookAt : MonoBehaviour
{
    public GameObject target;
    public float delay;
    private void Update()
    {
        StartCoroutine(LookAtTarget(target, delay));
    }

    private IEnumerator LookAtTarget(GameObject _target, float _delay = 0.5f)
    {
        var curPos = _target.transform.position;
        yield return new WaitForSeconds(_delay);
        gameObject.transform.LookAt(curPos);
    }
}
