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
        Debug.Log("Cubenotespawner: " +songData.title);
        for (int i = 0; i < songData.noteList.Count; i++)
        {
            Debug.Log("LineLayer: " + songData.noteList[i].lineLayer + " Index:" + songData.noteList[i].lineIndex + " timestamp: " + songData.noteList[i].timestamp + " direction: " + songData.noteList[i].cutDirection + " type: " + songData.noteList[i].type);
        }
        foreach (NoteData note in songData.noteList)
        {
            float cubeTravelTime = (20 / songData.noteSpeed);
            if (note.timestamp < cubeTravelTime) // skip cubeNotes that would spawn before the song has started
            {
                Debug.Log("skiped notes: " + note.timestamp);
                continue;
            }

            // Wait until the right time to spawn the Cubenote
            float spawnTime = note.timestamp - playerOffsetTime - cubeTravelTime;
            while (musicManager.GetCurrentTime() < spawnTime)
            {
                yield return null;
            }
            Debug.Log("spawntime: " + spawnTime + " timestamp: " + note.timestamp + " cubeTravelTime: " + cubeTravelTime);
            SpawnNote(note);
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
                rotation = Quaternion.Euler(0, 90, 0);
                break;
            case 1: // Down
                rotation = Quaternion.Euler(0, 90, 180);
                break;
            case 2: // Left
                rotation = Quaternion.Euler(90, 0, 90);
                break;
            case 3: // Right
                rotation = Quaternion.Euler(0, 90, -90);
                break;
            case 4: // Up Left
                rotation = Quaternion.Euler(90, 0, 45);
                break;
            case 5: // Up Right
                rotation = Quaternion.Euler(90, 0, -45);
                break;
            case 6: // Down Left
                rotation = Quaternion.Euler(90, 0, 135);
                break;
            case 7: // Down Right
                rotation = Quaternion.Euler(0, 90, -135);
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
