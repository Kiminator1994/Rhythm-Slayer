using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;


public class SongMapImporter : MonoBehaviour
{
    [System.Serializable]
    public class BeatSaberNote
    {
        public float _time;
        public int _lineIndex;
        public int _lineLayer;
        public int _type; // 0 for red, 1 for blue
        public int _cutDirection;
    }

    [System.Serializable]
    public class BeatSaberEvent
    {
        public float _time;
        public int _type;
        public int _value;
    }

    [System.Serializable]
    public class BeatSaberObstacle
    {
        public float _time;
        public int _lineIndex;
        public int _type;
        public float _duration;
        public int _width;
    }

    [System.Serializable]
    public class BeatSaberMap
    {
        public List<BeatSaberNote> _notes;
        public List<BeatSaberEvent> _events;
        public List<BeatSaberObstacle> _obstacles;
    }

    public BeatSaberMap LoadMap(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<BeatSaberMap>(json);
    }

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
