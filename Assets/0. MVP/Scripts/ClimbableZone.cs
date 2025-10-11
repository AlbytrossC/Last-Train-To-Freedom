using System;
using UnityEngine;

public class ClimbableZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Entered ClimbableZone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Exited ClimbableZone");
        }
    }
}
