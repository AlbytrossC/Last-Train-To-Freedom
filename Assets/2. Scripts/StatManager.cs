using UnityEngine;

public class StatManager
{
    #region Deaths
    public static void SetDeaths(int x) => PlayerPrefs.SetInt("Deaths", x);
    public static int GetDeaths() => PlayerPrefs.GetInt("Deaths", -1);
    public static void AddDeath(int x = 1) => PlayerPrefs.SetInt("Deaths", PlayerPrefs.GetInt("Deaths+" + x));
    #endregion
}
