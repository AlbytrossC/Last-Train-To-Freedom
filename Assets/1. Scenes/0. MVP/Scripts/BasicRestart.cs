using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicRestart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
<<<<<<< Updated upstream:Assets/1. Scenes/0. MVP/Scripts/BasicRestart.cs
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
=======
            FindFirstObjectByType<GameManager>().RestartLevel();
>>>>>>> Stashed changes:Assets/0. MVP/Scripts/BasicRestart.cs
        }
    }
}
