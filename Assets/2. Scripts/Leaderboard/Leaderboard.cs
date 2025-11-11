using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
public class Leaderboard : MonoBehaviour
{
    #region Script Stuff
    [Serializable]
    public struct LBEntry
    {
        public float  rank;
        public string name; // player name
        public float  time; // level completion time (seconds)
        //public float  deaths; // number of deaths

        public LBEntry(float rank, string name, float time)
        {
            this.rank = rank;
            this.name = name;
            this.time = time;
        }
    }
    private readonly List<LBEntry> _lbEntries = new List<LBEntry>();
    public static Leaderboard Instance {get; private set;}
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("[Leaderboard] Duplicate instance destroyed.");
            Destroy(this);
            return;
        }

        Instance = this;
    }
    
    
    public List<LBEntry> GetLeaderboard() => new List<LBEntry>(_lbEntries);

    public List<LBEntry> GetTop(int count)
    {
        if (count <= 0) return new List<LBEntry>();
        int take = Math.Min(count, _lbEntries.Count);
        return _lbEntries.GetRange(0, take);
    }
    public int AddEntry(string name, float time)
    {
        if (string.IsNullOrWhiteSpace(name) || name.Length != 3)
        {
            Debug.LogError($"[LeaderboardByRank] Name \"{name}\" must be exactly 3 letters.");
            return -1;
        }
        
        _lbEntries.Add(new LBEntry(0, name, time)); // temp rank = 0
        SortAndAssignRanks();
        
        int newRank = _lbEntries.FindIndex(e => e.name.Equals(name, StringComparison.OrdinalIgnoreCase)) + 1;
        return newRank;
    }
    public bool RemoveEntryByRank(int rank)
    {
        int idx = _lbEntries.FindIndex(e => e.rank == rank);
        if (idx < 0) return false;

        _lbEntries.RemoveAt(idx);
        SortAndAssignRanks(); 
        return true;
    }
    private void SortAndAssignRanks()
    {
        
        _lbEntries.Sort((a, b) => a.time.CompareTo(b.time));

        
        for (int i = 0; i < _lbEntries.Count; i++)
        {
            LBEntry e = _lbEntries[i];
            e.rank = i + 1;
            _lbEntries[i] = e;
        }
    }
    #endregion Script Stuff
    #region Unity Inspector Stuff
    [Header("=== Leaderboard Manager ===")]
    [Tooltip("3-letter name entry (E.g. AAA).")]
    [HideInInspector]
    public string inspectorNewName = "AAA";

    [Tooltip("Time (seconds) for the entry you want to add.")]
    [HideInInspector]
    public float inspectorNewTime = 0f;

    [Tooltip("Rank of the entry you want to remove (1 = best).")]
    [HideInInspector]
    public int inspectorRemoveRank = 1;
    
    public void Inspector_AddEntry()
    {
        int rank = AddEntry(inspectorNewName, inspectorNewTime);
        if (rank > 0)
            Debug.Log($"[Leaderboard] Added \"{inspectorNewName}\" with time {inspectorNewTime:F2}s → Rank #{rank}");
    }

    public void Inspector_RemoveEntry()
    {
        bool ok = RemoveEntryByRank(inspectorRemoveRank);
        Debug.Log(ok
            ? $"[Leaderboard] Removed entry at rank {inspectorRemoveRank}"
            : $"[Leaderboard] No entry at rank {inspectorRemoveRank}");
    }
    #endregion Unity Inspector Stuff
}
