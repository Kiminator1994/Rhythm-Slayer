using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData
{
    public List<SongSaveData> songSaveDatas;
    public SettingsSaveData settingsSaveData;

    public SaveData()
    {
        songSaveDatas = new List<SongSaveData>();
        settingsSaveData = new SettingsSaveData();
    }
}

[System.Serializable]
public class SongSaveData
{
    public string songTitle;
    public int highScore;
    public int maxCombo;

    public SongSaveData(string songTitle, int highScore, int maxCombo)
    {
        this.songTitle = songTitle;
        this.highScore = highScore;
        this.maxCombo = maxCombo;
    }
}

[System.Serializable]
public class SettingsSaveData
{
    public float playerOffset;
    public int selectedResolutionIndex;
    public bool isFullscreen;
    public bool vsync;

    public SettingsSaveData()
    {
        playerOffset = 0f;
        selectedResolutionIndex = 0;
        isFullscreen = true;
        vsync = false;
    }
}