using System;
using System.Collections;
using UnityEngine;

public class ClimbableZone : MonoBehaviour
{
    public PlayerController playerController;
    public GameObject player;
    public GameObject popupText;

    public GameObject teleTopPos;
    private float topY;
    public GameObject midHeight;
    private float midHeightY;
    public GameObject teleBotPos;
    private float botY;
    public GameObject ExitTopPos;
    public GameObject ExitBotPos;
    private Collider col;
    
    

    private void Start()
    {
        midHeightY = midHeight.transform.position.y;
        col = GetComponent<Collider>();
        popupText.SetActive(false);
        topY = teleTopPos.transform.position.y;
        botY = teleBotPos.transform.position.y;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("Entered Climb Zone");
            popupText.SetActive(true);
            playerController.AllowToggleClimb();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            popupText.SetActive(false);
            print("Exited Climb Zone");
            playerController.DenyToggleClimb();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerController.startClimb)
            {
                playerController.startClimb = false;
                StartClimbing();
            }
        }
    }

    private void Update()
    {
        if (playerController.isClimbing)
        {
            if (player.transform.position.y < (botY - 1))
                EndClimbing(false);
            if (player.transform.position.y > (topY + 1))
                EndClimbing(true);
        }
    }

    private void StartClimbing()
    {
        playerController.TogglePlayerClimb();
        popupText.SetActive(false);
        if (player.transform.position.y > midHeightY)
            player.transform.position = teleTopPos.transform.position;
        if (player.transform.position.y < midHeightY)
            player.transform.position = teleBotPos.transform.position;
    }
    private void EndClimbing(bool onTop)
    {
        switch (onTop)
        {
            case true:
                player.transform.position = ExitTopPos.transform.position;
                break;
            case false:
                player.transform.position = ExitBotPos.transform.position;
                break;
        }
        playerController.TogglePlayerClimb();
        StartCoroutine(ClimbCooldown());
        player.GetComponent<Rigidbody>().linearVelocity = player.GetComponent<Rigidbody>().linearVelocity * 0.5f;
    }

    private IEnumerator ClimbCooldown()
    {
        col.enabled = false;
        yield return new WaitForSeconds(1.2f);
        col.enabled = true;
    }
}
