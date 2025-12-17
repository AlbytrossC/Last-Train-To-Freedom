using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicRestart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindFirstObjectByType<GameManager>().RestartLevel();
        }
    }
}
