using System;
using UnityEngine;

public class ResetDeaths : MonoBehaviour
{
    public void SetDeaths()
    {
        StatManager.SetDeaths(0);
    }
}
