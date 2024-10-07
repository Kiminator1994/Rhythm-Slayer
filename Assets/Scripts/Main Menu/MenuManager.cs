using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadRandomSongScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SelectMusic()
    {

    }

    public void EndGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        Debug.Log("Game is exiting...");
#endif
    }
}
