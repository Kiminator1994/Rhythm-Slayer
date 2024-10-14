using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;

public class NoteSpawnManager : MonoBehaviour
{
    private const byte TotalSpawnPoints = 15; // Total number of spawn points
    private const byte MaxPatternOffset = 7;  // Maximum offset for patterns
    private const int TotalBeats = 428;       // Total beats based on song duration and BPM
    private List<byte[]> patterns = new List<byte[]>();

    public List<Transform> spawnPoints;  // Reference to all your spawn points
    public GameObject notePrefab;        // Note prefab to spawn

    public float BPM = 138f;             // Beats per minute for the song
    public float noteSpeed = 5f;         // Speed of the notes moving towards the player

    // Calculated BeatInterval based on BPM
    private float BeatInterval => 60f / BPM;


    void Start()
    {
        GeneratePatterns(); // Generate the note patterns
        StartCoroutine(SpawnNotes());
    }

    // Generates all note patterns programmatically.
    private void GeneratePatterns()
    {
        GenerateAdjacentPatterns();
        GenerateFixedSpacingPatterns(3, MaxPatternOffset);  // Create patterns with fixed offsets
        GenerateDiagonalPatterns(5); // Changed spacing for more variety
        GenerateCustomPatterns();

        // Extend patterns to cover the total number of beats
        ExtendPatternsToCoverTotalBeats(TotalBeats);
    }

    private void GenerateAdjacentPatterns()
    {
        for (byte i = 0; i < TotalSpawnPoints - 1; i += 2)
        {
            patterns.Add(new byte[] { i, (byte)(i + 1) });
        }
    }

    private void GenerateFixedSpacingPatterns(byte spacing1, byte spacing2)
    {
        for (byte i = 0; i <= 7; i++)  // Adjusted for 15 spawn points
        {
            patterns.Add(new byte[] { i, (byte)(i + spacing1) });
            patterns.Add(new byte[] { i, (byte)(i + spacing2) });
        }
    }

    private void GenerateDiagonalPatterns(byte spacing)
    {
        for (byte i = 0; i <= 7; i++) // Adjusted for 15 spawn points
        {
            patterns.Add(new byte[] { i, (byte)(i + spacing) });
        }
    }

    private void GenerateCustomPatterns()
    {
        patterns.AddRange(new List<byte[]>
        {
            new byte[] { 0, 12 }, // Custom pattern 1
            new byte[] { 2, 13 }, // Custom pattern 2
            new byte[] { 5, 9 },  // Custom pattern 3
            new byte[] { 7, 14 }, // Custom pattern 4
            new byte[] { 3, 8 }   // Custom pattern 5
        });
    }

    private void ExtendPatternsToCoverTotalBeats(int totalBeats)
    {
        int currentPatternCount = patterns.Count;

        // Repeat and mix patterns to fill the remaining beats
        for (int i = currentPatternCount; i < totalBeats; i++)
        {
            patterns.Add(patterns[i % currentPatternCount]); // Loop over existing patterns
        }
    }

    // Coroutine to spawn notes based on patterns at regular intervals.
    private IEnumerator SpawnNotes()
    {
        foreach (var pattern in patterns)
        {
            yield return new WaitForSeconds(BeatInterval); // Spawn notes at each beat

            foreach (byte spawnPoint in pattern)
            {
                SpawnNoteAtPoint(spawnPoint);
            }
        }
    }

    // Instantiates a note at the given spawn point and applies velocity towards the player.
    private void SpawnNoteAtPoint(byte spawnPoint)
    {
        if (spawnPoint < spawnPoints.Count)  // in case of a bug, or typo when creating new patterns
        {
            GameObject note = Instantiate(notePrefab, spawnPoints[spawnPoint].position, notePrefab.transform.rotation);
            Rigidbody rb = note.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.velocity = new Vector3(0, 0, -noteSpeed); // Move the note toward the player (-Z direction)
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
