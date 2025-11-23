using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Chart))]
public class ChartEditor : Editor
{
    SerializedProperty bpmProp;
    SerializedProperty musicProp;
    SerializedProperty notesProp;

    static readonly float[] snapOptions = { 1f, 0.5f, 0.25f, 0.125f };
    static int snapIndex = 0;

    void OnEnable()
    {
        bpmProp = serializedObject.FindProperty("bpm");
        musicProp = serializedObject.FindProperty("musicClip");
        notesProp = serializedObject.FindProperty("notes");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(musicProp);
        EditorGUILayout.PropertyField(bpmProp);

        EditorGUILayout.Space(15);

        // Calculate total beats
        float totalBeats = 0f;
        Chart chart = (Chart)target;

        if (chart.musicClip != null && chart.bpm > 0)
        {
            totalBeats = chart.musicClip.length * chart.bpm / 60f;
        }

        EditorGUILayout.LabelField("Total Beats in Song", totalBeats.ToString("F1"));

        snapIndex = EditorGUILayout.Popup("Snap", snapIndex, new[] { "1/1", "1/2", "1/4", "1/8" });

        EditorGUILayout.Space(15);

        EditorGUILayout.LabelField("Notes", EditorStyles.boldLabel);

        for (int i = 0; i < notesProp.arraySize; i++)
        {
            SerializedProperty note = notesProp.GetArrayElementAtIndex(i);

            EditorGUILayout.BeginVertical();

            SerializedProperty beat = note.FindPropertyRelative("beat");
            beat.floatValue = EditorGUILayout.FloatField("Beat", beat.floatValue);
            beat.floatValue = SnapBeat(beat.floatValue);
            beat.floatValue = Mathf.Clamp(beat.floatValue, 1f, totalBeats);

            SerializedProperty lane = note.FindPropertyRelative("lane");
            lane.intValue = EditorGUILayout.IntSlider("Lane", lane.intValue, 1, GlobalValue.LandCount);

            SerializedProperty isHold = note.FindPropertyRelative("isLongNote");
            isHold.boolValue = EditorGUILayout.Toggle("Is Long Note", isHold.boolValue);

            if(isHold.boolValue)
            {
                SerializedProperty duration = note.FindPropertyRelative("duration");
                duration.floatValue = EditorGUILayout.FloatField("Duration (beats)", duration.floatValue);
                
                duration.floatValue = Mathf.Round(duration.floatValue / snapOptions[snapIndex]) * snapOptions[snapIndex];
            }

            // Delete button
            if (GUILayout.Button("Delete Note"))
            {
                notesProp.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(10);
        }

        EditorGUILayout.Space(15);

        if (GUILayout.Button("Add Note"))
        {
            notesProp.InsertArrayElementAtIndex(notesProp.arraySize);
        }

        if (GUILayout.Button("Sort Notes On Beat"))
        {
            //Chart chart = (Chart)target;
            chart.notes.Sort((a, b) => a.beat.CompareTo(b.beat));
            EditorUtility.SetDirty(chart);
        }

        if(GUILayout.Button("Sort Notes On Lane"))
        {
            //Chart chart = (Chart)target;
            chart.notes.Sort((a, b) => a.lane.CompareTo(b.lane));
            EditorUtility.SetDirty(chart);
        }

        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("Mini Timeline Preview", EditorStyles.boldLabel);

        // Reserve space for drawing
        float pixelsPerBeat = 15f;
        float previewHeight = totalBeats * pixelsPerBeat;
        Rect previewRect = GUILayoutUtility.GetRect(0, previewHeight, GUILayout.ExpandWidth(true));
        DrawTimeline(previewRect, chart);
        EditorGUILayout.Space(10);

        serializedObject.ApplyModifiedProperties();
    }

    private float SnapBeat(float input)
    {
        float snapValue = snapOptions[snapIndex];
        return Mathf.Round(input / snapValue) * snapValue;
    }

    private void DrawTimeline(Rect rect, Chart chart)
    {
        if (chart == null || chart.notes == null) return;

        EditorGUI.DrawRect(rect, Color.black);

        int laneCount = 4;
        float laneWidth = rect.width / laneCount;

        // Draw lane dividers
        for (int i = 1; i < laneCount; i++)
        {
            float x = rect.x + i * laneWidth;
            EditorGUI.DrawRect(new Rect(x - 1, rect.y, 2, rect.height), Color.gray);
        }

        float maxBeat = chart.musicClip != null && chart.bpm > 0
            ? chart.musicClip.length * chart.bpm / 60f
            : 1f; // avoid divide by zero

        // Draw notes
        foreach (var note in chart.notes)
        {
            float x = rect.x + (note.lane - 1 + 0.5f) * laneWidth - 5;

            float yStart = rect.y + rect.height - (note.beat / maxBeat) * rect.height;
            float yEnd = yStart;

            if (note.isLongNote)
            {
                yEnd = rect.y + rect.height - ((note.beat + note.duration) / maxBeat) * rect.height;
                EditorGUI.DrawRect(new Rect(x, yEnd + 5, 10, yStart - yEnd), Color.yellow);
            }
            else
            {
                Rect noteRect = new Rect(x, yStart - 5, 10, 10);
                EditorGUI.DrawRect(noteRect, Color.cyan);
            }
        }
    }


}
