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

    public float playerOffsetTime = 0f; // Adjustable time offset for the player in settings TO DO!!!!

    private void Start()
    {
        spawnPoints = new Transform[,]
        {
            { layerOne[0], layerTwo[0], layerThree[0] },
            { layerOne[1], layerTwo[1], layerThree[1] },
            { layerOne[2], layerTwo[2], layerThree[2] },
            { layerOne[3], layerTwo[3], layerThree[3] }
        };

        songData = GameManager.Instance.GetSongData();
        if (songData != null)
            StartCoroutine(SpawnNotes());

    }

    private IEnumerator SpawnNotes()
    {
        float cubeTravelTime = (20 / songData.noteSpeed);  // Calculate cube travel time based on note speed

        Debug.Log("Cubenotespawner: " + songData.title + " Notes: " + songData.noteList.Count);

        foreach (NoteData note in songData.noteList)
        {
            if(GameManager.Instance.GameIsOver)
            {
                break;
            }

            // Convert timestamp from beats to seconds using BPM
            float secondsPerBeat = 60f / songData.bpm;
            float spawnTimeInSeconds = note.timestamp * secondsPerBeat;

            float spawnTime = spawnTimeInSeconds + playerOffsetTime + songData.songTimeOffset - cubeTravelTime;
            float remainingTime = (GameManager.Instance.GetRemainingCountdown() * -1); // * -1 so we have negative Time since we get negative time in the beginning from cubeTravelTime
            Debug.Log("spawntime: " + spawnTime + " remainingtime: " + remainingTime);
            if (spawnTime < remainingTime && spawnTime <= 0)
            {
                SpawnNote(note);
                continue;
            }
            else
            {
                // Wait until it's time to spawn the note
                while (musicManager.GetCurrentTime() < spawnTime)
                {
                    yield return null;  // Wait until the exact time to spawn the note
                }
                SpawnNote(note);
            }

        }
    }

    // Spawns the note at the correct spawn point with the correct color
    private void SpawnNote(NoteData note)
    {
        GameObject notePrefab = note.type == 1 ? redNotePrefab : blueNotePrefab;

        // Define a dictionary to store cut direction rotations
        Quaternion rotation = notePrefab.transform.rotation; // Default rotation

        switch (note.cutDirection)
        {
            case 0: // Up
                rotation = Quaternion.Euler(180, 270, 0);
                break;
            case 1: // Down
                rotation = Quaternion.Euler(0, 270, 0);
                break;
            case 2: // Left
                rotation = Quaternion.Euler(-90, 270, 0);
                break;
            case 3: // Right
                rotation = Quaternion.Euler(90, 270, 0);
                break;
            case 4: // Up Left
                rotation = Quaternion.Euler(-135, 270, 0);
                break;
            case 5: // Up Right
                rotation = Quaternion.Euler(135, 270, 0);
                break;
            case 6: // Down Left
                rotation = Quaternion.Euler(-45, 270, 0);
                break;
            case 7: // Down Right
                rotation = Quaternion.Euler(45, 270, 0);
                break;
            case 8: // Any
                if (note.type == 1)
                    notePrefab = redAnyDirectionNotePrefab;
                else
                    notePrefab = blueAnyDirectionNotePrefab;
                break;
        }
        Debug.Log("type: " + note.type + " cutDir: " + note.cutDirection + " index: " + note.lineIndex + " layer: " + note.lineLayer);
        Transform spawnPoint = spawnPoints[note.lineIndex, note.lineLayer];
        Instantiate(notePrefab, spawnPoint.position, rotation);
    }
}
