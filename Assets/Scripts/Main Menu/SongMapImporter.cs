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
