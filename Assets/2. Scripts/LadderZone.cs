using UnityEngine;
using UnityEngine.Events;

public class LadderZone : MonoBehaviour
{
    public UnityEvent onLadderZoneEnter;
    public UnityEvent onLadderZoneExit;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            onLadderZoneEnter.Invoke();
            print("Entered LadderZone");
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onLadderZoneExit.Invoke();
            print("Exited LadderZone");
        }
    }
}
