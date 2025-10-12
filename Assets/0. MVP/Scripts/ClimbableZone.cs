using System;
using UnityEngine;

public class ClimbableZone : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject player;
    public GameObject popupText;

    public GameObject teleTopPos;
    public GameObject teleBotPos;
    
    

    private void Start()
    {
        popupText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupText.SetActive(true);
            playerController.AllowToggleClimb();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupText.SetActive(true);
            print("Exited ClimbableZone");
            playerController.DenyToggleClimb();
        }
    }
    
    private void StartClimbing()
    {
        popupText.SetActive(false);
        player.transform.position = teleBotPos.transform.position;
    }
    private void EndClimbing()
    {
        popupText.SetActive(false);
        player.transform.position = teleBotPos.transform.position;
    }
}
