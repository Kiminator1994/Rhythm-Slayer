using System;
using System.Collections.Generic;
using UnityEngine;
using static SongMapImporter;

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



