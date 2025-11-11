using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Leaderboard))]
public class LeaderboardEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        EditorGUILayout.Space();
        
        Leaderboard lb = (Leaderboard)target;

        // ----------
        // ADD ENTRY
        // ----------
        
        EditorGUILayout.LabelField("=== Add New Entry ===", EditorStyles.boldLabel);
        lb.inspectorNewName = EditorGUILayout.TextField("Name (3 letters)", lb.inspectorNewName);
        lb.inspectorNewTime = EditorGUILayout.FloatField("Time (seconds)", lb.inspectorNewTime);
        
        if (GUILayout.Button("Add Entry"))
        {
            Undo.RecordObject(lb, "Add Leaderboard Entry");
            lb.Inspector_AddEntry();
            EditorUtility.SetDirty(lb);
        }

        // -------------
        // REMOVE ENTRY
        // -------------
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("=== Remove Entry By # ===", EditorStyles.boldLabel);
        lb.inspectorRemoveRank = EditorGUILayout.IntSlider(lb.inspectorRemoveRank, 1, 10);

        if (GUILayout.Button("Remove Entry"))
        {
            Undo.RecordObject(lb, "Remove Leaderboard Entry");
            lb.Inspector_RemoveEntry();
            EditorUtility.SetDirty(lb);
        }

        // --------------------------------------------------------------
        // DISPLAY CURRENT LEADERBOARD (read‑only)
        // --------------------------------------------------------------
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("=== Current Leaderboard (read‑only) ===", EditorStyles.boldLabel);

        // Show each entry in a compact, read‑only fashion
        var entries = lb.GetLeaderboard();
        if (entries.Count == 0)
        {
            EditorGUILayout.LabelField("(empty)");
        }
        else
        {
            // Header row
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rank", GUILayout.Width(40));
            EditorGUILayout.LabelField("Name", GUILayout.Width(50));
            EditorGUILayout.LabelField("Time (s)", GUILayout.Width(70));
            EditorGUILayout.EndHorizontal();

            // Data rows (disabled GUI so they aren't editable)
            EditorGUI.BeginDisabledGroup(true);
            foreach (var e in entries)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(e.rank.ToString(), GUILayout.Width(40));
                EditorGUILayout.LabelField(e.name, GUILayout.Width(50));
                EditorGUILayout.LabelField(e.time.ToString("F2"), GUILayout.Width(70));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.EndDisabledGroup();
        }

        // Force a repaint when something changes (helps the list stay up‑to‑date)
        if (GUI.changed) Repaint();
    }
}
