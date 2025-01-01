using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class SongMapImporter : MonoBehaviour
{
    [System.Serializable]
    public class BeatNote
    {
        public float _time;
        public int _lineIndex;
        public int _lineLayer;
        public int _type;
        public int _cutDirection;
    }

    [System.Serializable]
    public class BeatEvent
    {
        public float _time;
        public int _type;
        public int _value;
    }

    [System.Serializable]
    public class BeatMap
    {
        public List<BeatNote> _notes;
        public List<BeatEvent> _events;
    }

    // Read Beatmapper file
    public BeatMap LoadMap(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<BeatMap>(json);
    }

    public List<NoteData> ConvertToNoteData(BeatMap beatMap)
    {
        List<NoteData> noteDataList = new List<NoteData>();

        foreach (var beatNote in beatMap._notes)
        {
            NoteData noteData = new NoteData(
                beatNote._time,
                beatNote._lineIndex,
                beatNote._lineLayer,
                beatNote._type,
                beatNote._cutDirection
            );
            noteDataList.Add(noteData);
        }
        return noteDataList;
    }

    public List<EventData> ConvertToEventData(BeatMap beatMap)
    {
        List<EventData> eventDataList = new List<EventData>();

        foreach (var beatEvent in beatMap._events)
        {
            EventData eventData = new EventData(
                beatEvent._time,
                beatEvent._type,
                beatEvent._value
            );
            eventDataList.Add(eventData);
        }
        return eventDataList;
    }
}



