using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SongMapImporter : MonoBehaviour
{
    [System.Serializable]
    public class BeatSaberNote
    {
        public float _time;
        public int _lineIndex;
        public int _lineLayer;
        public int _type;
        public int _cutDirection;
    }

    [System.Serializable]
    public class BeatSaberMap
    {
        public List<BeatSaberNote> _notes;
    }

    [System.Serializable]
    public class BeatSaberDifficulty
    {
        public string _difficulty;
        public float _noteJumpMovementSpeed;
        public string _beatmapFilename;
    }

    [System.Serializable]
    public class BeatSaberDifficultyBeatmapSet
    {
        public string _beatmapCharacteristicName;
        public List<BeatSaberDifficulty> _difficultyBeatmaps;
    }

    [System.Serializable]
    public class BeatSaberInfo
    {
        public string _songName;
        public string _songAuthorName;
        public float _beatsPerMinute;
        public float _songTimeOffset;
        public List<BeatSaberDifficultyBeatmapSet> _difficultyBeatmapSets;
    }

    // Load map for a specific difficulty level
    public BeatSaberMap LoadMap(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<BeatSaberMap>(json);
    }

    // Load the Info.dat file and return song details including the note speed
    public BeatSaberInfo LoadInfo(string filePath)
    {
        string json = File.ReadAllText(filePath);

        // DEBUG: Print the contents of Info.dat
        Debug.Log("Loading Info.dat: " + json);

        // Deserialize and return
        BeatSaberInfo beatSaberInfo = JsonConvert.DeserializeObject<BeatSaberInfo>(json);

        // DEBUG: Check parsed data
        Debug.Log($"Parsed Info.dat: SongName = {beatSaberInfo._songName}, Author = {beatSaberInfo._songAuthorName}, BPM = {beatSaberInfo._beatsPerMinute}");

        return beatSaberInfo;
    }

    public float GetNoteJumpMovementSpeed(BeatSaberInfo beatSaberInfo)
    {
        // Get the first available difficulty and extract the note jump movement speed
        foreach (var beatmapSet in beatSaberInfo._difficultyBeatmapSets)
        {
            if (beatmapSet._difficultyBeatmaps != null && beatmapSet._difficultyBeatmaps.Count > 0)
            {
                // Get the note jump movement speed from the first difficulty
                BeatSaberDifficulty firstDifficulty = beatmapSet._difficultyBeatmaps[0];
                return firstDifficulty._noteJumpMovementSpeed;
            }
        }

        return 0f;  // Return a default value if no note speed is found
    }

    // Convert BeatSaberMap notes into NoteData list
    public List<NoteData> ConvertToNoteData(BeatSaberMap beatSaberMap)
    {
        List<NoteData> noteDataList = new List<NoteData>();

        foreach (var beatSaberNote in beatSaberMap._notes)
        {
            NoteData noteData = new NoteData(beatSaberNote._time, beatSaberNote._lineIndex, beatSaberNote._lineLayer, beatSaberNote._type, beatSaberNote._cutDirection);
            noteDataList.Add(noteData);
        }
        return noteDataList;
    }
}
