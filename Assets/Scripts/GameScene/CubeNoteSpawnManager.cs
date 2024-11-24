using System.Collections;
using UnityEngine;

public class CubeNoteSpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] layerOne = new Transform[4];
    [SerializeField] private Transform[] layerTwo = new Transform[4];
    [SerializeField] private Transform[] layerThree = new Transform[4];
    private Transform[,] spawnPoints;


    public GameObject redNotePrefab;
    public GameObject blueNotePrefab;

    public GameObject redAnyDirectionNotePrefab;
    public GameObject blueAnyDirectionNotePrefab;

    public MusicManager musicManager;
    public SongData songData;

    public float playerOffsetTime = 0f;

    private void Start()
    {
        spawnPoints = new Transform[,]
        {
            { layerOne[0], layerTwo[0], layerThree[0] },
            { layerOne[1], layerTwo[1], layerThree[1] },
            { layerOne[2], layerTwo[2], layerThree[2] },
            { layerOne[3], layerTwo[3], layerThree[3] }
        };
        playerOffsetTime = GameManager.Instance.GetPlayerOffset();
        Debug.Log("playerOffset: " + playerOffsetTime);
        songData = GameManager.Instance.GetSongData();
        if (songData != null)
            StartCoroutine(SpawnNotes());
    }

    private IEnumerator SpawnNotes()
    {
        float cubeTravelTime = (20 / songData.noteSpeed);  // Calculate cube travel time based on note speed to reach Player

        foreach (NoteData note in songData.noteList)
        {
            if(GameManager.Instance.GameIsOver)
            {
                break;
            }

            // Music Games work with Beats instead of seconds. Means timestamp is the actual beat in the song
            // To spawn the cubes at the correct time, we have to calculate the spawntime according to the the bpm.
            float secondsPerBeat = 60f / songData.bpm;
            float spawnTimeInSeconds = note.timestamp * secondsPerBeat;
            float spawnTime = spawnTimeInSeconds + playerOffsetTime + songData.songTimeOffset - cubeTravelTime;

            if (spawnTime < 0)
            {
                // Wait until it's time to spawn the note
                while ((GameManager.Instance.GetRemainingCountdown() *-1) < spawnTime)
                {
                    yield return null;
                }
                SpawnNote(note);
            }
            else
            {
                // Wait until it's time to spawn the note
                while (musicManager.GetCurrentTime() < spawnTime)
                {
                    yield return null; 
                }
                SpawnNote(note);
            }
        }
    }

    // Spawns the note at the correct spawn point with the correct color and rotation
    private void SpawnNote(NoteData note)
    {
        GameObject notePrefab = note.type == 1 ? redNotePrefab : blueNotePrefab;

        if (note.type > 1) // we only handle normal cubes for the moment, special types will be implemented later
        {
            return;
        }

        Quaternion rotation = notePrefab.transform.rotation; // Default rotation

        switch (note.cutDirection)
        {
            case 0: // Up
                rotation = Quaternion.Euler(0, 0, 0);
                break;
            case 1: // Down
                rotation = Quaternion.Euler(0, 0, 180);
                break;
            case 2: // Left
                rotation = Quaternion.Euler(0, 0, 90);
                break;
            case 3: // Right
                rotation = Quaternion.Euler(0, 0, -90);
                break;
            case 4: // Up Left
                rotation = Quaternion.Euler(0, 0, 45);
                break;
            case 5: // Up Right
                rotation = Quaternion.Euler(0, 0, -45);
                break;
            case 6: // Down Left
                rotation = Quaternion.Euler(0, 0, 135);

                break;
            case 7: // Down Right
                rotation = Quaternion.Euler(0, 0, -135);
                break;
            case 8: // Any
                if (note.type == 1)
                    notePrefab = redAnyDirectionNotePrefab;
                else
                    notePrefab = blueAnyDirectionNotePrefab;
                break;
        }
        Instantiate(notePrefab, spawnPoints[note.lineIndex, note.lineLayer].position, rotation);
    }
}
