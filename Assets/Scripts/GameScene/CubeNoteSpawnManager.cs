using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeNoteSpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints;      // List of spawn points
    public GameObject redNotePrefab;         // Red note prefab
    public GameObject blueNotePrefab;        // Blue note prefab

    public MusicManager musicManager;        // Reference to MusicManager to get song time
    public SongData songData;                // Loaded song data with notes and timestamps

    public float noteSpeed = 5f;             // Speed of the notes towards the player
    public float playerOffsetTime = 0f;      // Adjustable time offset for the player

    private void Start()
    {
        if (songData != null)
            StartCoroutine(SpawnNotes());  // Start spawning notes
    }

    // Coroutine to spawn notes based on timestamps
    private IEnumerator SpawnNotes()
    {
        foreach (NoteData note in songData.noteList)
        {
            float cubeTravelTime = (20 / noteSpeed);
            if (note.timestamp < cubeTravelTime)
            {
                continue;
            }

            // Wait until the right time to spawn the note
            float spawnTime = note.timestamp - playerOffsetTime - cubeTravelTime;
            while (musicManager.GetCurrentTime() < spawnTime)
            {
                yield return null;  // Wait until the song time reaches the timestamp minus the offset
            }

            SpawnNoteAtPoint(note.spawnPoint, note.isBlueCube);
        }
    }

    // Spawns the note at the correct spawn point with the correct color
    private void SpawnNoteAtPoint(byte spawnPoint, bool isBlueCube)
    {
        if (spawnPoint < spawnPoints.Count)
        {
            GameObject notePrefab = isBlueCube ? blueNotePrefab : redNotePrefab;
            GameObject note = Instantiate(notePrefab, spawnPoints[spawnPoint].position, notePrefab.transform.rotation);

            Rigidbody rb = note.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = new Vector3(0, 0, -noteSpeed);  // Move note towards the player (negative Z direction)
            }
            else
            {
                Debug.LogWarning("Note prefab is missing a Rigidbody component!");
            }
        }
        else
        {
            Debug.LogWarning($"Spawn point index {spawnPoint} is out of range.");
        }
    }
}
