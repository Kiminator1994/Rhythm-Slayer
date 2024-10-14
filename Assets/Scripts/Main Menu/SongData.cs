using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSong", menuName = "SongLibrary/Song")]
public class SongData : ScriptableObject
{
    public string interpret;
    public string title;
    public AudioClip audioClip;
    public float bpm;
    public Sprite backgroundImage;
    public int highScore;
    public int maxCombo;
    public List<NoteData> noteList;
}

[System.Serializable]
public class NoteData
{
    [SerializeField] public float timestamp;   // Timestamp for the note spawn

    [SerializeField] public byte spawnPoint;   // Spawn point index

    [SerializeField] public bool isBlueCube;   // Cube color: 0 for red, 1 for blue

    public NoteData(float timestamp, byte spawnPoint, bool isBlueCube)
    {
        this.timestamp = timestamp;
        this.spawnPoint = spawnPoint;
        this.isBlueCube = isBlueCube;
    }
}

