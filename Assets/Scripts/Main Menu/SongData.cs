using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSong", menuName = "SongLibrary/Song")]
public class SongData : ScriptableObject
{
    [SerializeField] public string interpret;
    [SerializeField] public string title;
    [SerializeField] public AudioClip audioClip;
    [SerializeField] public float bpm;
    [SerializeField] public Sprite backgroundImage;
    [SerializeField] public int highScore;
    [SerializeField] public int maxCombo;
    [SerializeField] public float noteSpeed;
    [SerializeField] public float songTimeOffset;
    [SerializeField] public List<NoteData> noteList;
    [SerializeField] public List<EventData> eventList;
    [SerializeField] public string noteDataFilePath;
    [SerializeField] public string difficulty;
}

[System.Serializable]
public class NoteData
{
    [SerializeField] public float timestamp;
    [SerializeField] [Range(0, 3)] public int lineIndex;
    [SerializeField] [Range(0, 2)] public int lineLayer;
    [SerializeField] [Range(0, 8)] public int cutDirection;
    [SerializeField] [Range(0, 1)] public int type;

    public NoteData(float timestamp, int lineIndex, int lineLayer, int type, int cutDirection)
    {
        this.timestamp = timestamp;
        this.lineIndex = lineIndex;
        this.lineLayer = lineLayer;
        this.type = type;
        this.cutDirection = cutDirection;
    }
}


[System.Serializable]
public class EventData
{
    public float time; // Beatstamp
    public int type; // 0: Back laser, 1: Track neons, 2: red laser, 3: blue laser, 4: Primary light
    public int value; //0: Laser off, 1: Blue on, 2: fade in blue, 3: fade out blue, 5: red On, 6: fade in red, 7: fade out red

    public EventData(float time, int type, int value)
    {
        this.time = time;
        this.type = type;
        this.value = value;
    }
}



