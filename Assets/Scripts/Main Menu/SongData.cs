using UnityEngine;

[CreateAssetMenu(fileName = "NewSong", menuName = "SongLibrary/Song")]
public class SongData : ScriptableObject
{
    public string title;
    public AudioClip audioClip;
    public float bpm;
    public Sprite backgroundImage;
    public int highScore;
    public int maxCombo;
}