using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class LeaderboardUIManager : MonoBehaviour
{
    public Leaderboard leaderboard;
    public RectTransform rowsParent;
    public GameObject rowPrefab;
    
    private const int MaxRows = 10;
    private readonly List<TMP_Text> _rowTexts = new List<TMP_Text>();
    
    private void Awake()
    {
        if (leaderboard == null)
            Debug.LogError("[LeaderboardUIManager] No Leaderboard reference assigned.", this);
        if (rowsParent == null)
            Debug.LogError("[LeaderboardUIManager] No Rows Parent assigned.", this);
        if (rowPrefab == null)
            Debug.LogError("[LeaderboardUIManager] No Row Prefab assigned.", this);
        CreateRows();
        RefreshDisplay();
    }
    
    public void RefreshDisplay()
    {
        if (leaderboard == null) return;
        List<Leaderboard.LBEntry> top = leaderboard.GetTop(MaxRows);
        
        for (int i = 0; i < MaxRows; i++)
        {
            TMP_Text txt = _rowTexts[i];
            if (i < top.Count)
            {
                var e = top[i];
                // Example format: "#1  ABC – 9.87s"
                txt.text = $"#{e.rank}  {e.name} – {e.time:F2}s";
                txt.gameObject.SetActive(true);
            }
            else
            {
                txt.text = $"--  --- – --.--s";
                txt.gameObject.SetActive(false);
            }
        }
    }
    
    private void CreateRows()
    {
        foreach (Transform child in rowsParent)
        {
            if (Application.isPlaying)
                Destroy(child.gameObject);
            else
                DestroyImmediate(child.gameObject);
        }
        
        for (int i = 0; i < MaxRows; i++)
        {
            GameObject go = Instantiate(rowPrefab, rowsParent);
            go.name = $"Row_{i + 1}";
            go.SetActive(true);

            TMP_Text txt = go.GetComponentInChildren<TMP_Text>(true);
            if (txt == null)
            {
                Debug.LogError($"[LeaderboardUIManager] Row prefab does not contain a TMP_Text component (row {i + 1}).", go);
                continue;
            }

            _rowTexts.Add(txt);
        }
    }
#if UNITY_EDITOR
    private void OnEnable()
    {
        UnityEditor.EditorApplication.delayCall += () =>
        {
            if (this != null) RefreshDisplay();
        };
    }
#endif
}