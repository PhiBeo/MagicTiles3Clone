using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Note
{
    public float beat;
    public int lane;
    public bool isLongNote;
    public float duration;
}

[CreateAssetMenu(fileName = "Chart")]
public class Chart : ScriptableObject
{
    public AudioClip musicClip;
    public float bpm;
    public List<Note> notes = new List<Note>();
}
