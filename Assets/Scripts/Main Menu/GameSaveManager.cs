using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSaveManager
{
    private static string SaveFilePath => Application.persistentDataPath + "/GameSaveData.json";

    public static void SaveGame(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(SaveFilePath, json);
        Debug.Log($"Game saved at {SaveFilePath}");
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            return JsonUtility.FromJson<SaveData>(json);
        }

        Debug.LogWarning("Save file not found. Returning new GameDataManager.");
        return new SaveData();
    }
}
