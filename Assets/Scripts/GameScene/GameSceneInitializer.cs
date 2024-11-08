using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    [SerializeField] private SwordInteraction swordLeftRef;
    [SerializeField] private SwordInteraction swordRightRef;
    [SerializeField] private UIManager hudManagerRef;
    [SerializeField] private NoteEndManager noteEndManagerRef;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private CubeNoteSpawnManager cubeNoteSpawnManagerRef;

    private void Awake()
    {
        Debug.Log("initialize Scene...");
        GameManager.Instance.GameIsOver = false;
        GameManager.Instance.SetGameSceneReferences(swordLeftRef, swordRightRef, hudManagerRef, noteEndManagerRef, musicManager);
        GameManager.Instance.SubcscribeGameSceneEvents();
        GameManager.Instance.ResetGameSceneStats();
    }

    private void Start()
    {
        cubeNoteSpawnManagerRef.songData = GameManager.Instance.GetSongData();
        hudManagerRef.StartCountdown(StartMusic);
        Debug.Log(GameManager.Instance.GetAudioClip());
        Debug.Log("Scene initialized");
    }

    private void StartMusic()
    {
        musicManager.PlayMusic(GameManager.Instance.GetAudioClip());
    }
}
