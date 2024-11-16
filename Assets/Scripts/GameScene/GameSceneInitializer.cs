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
        hudManagerRef.StartCountdown(StartMusic);
        cubeNoteSpawnManagerRef.songData = GameManager.Instance.GetSongData();
        Debug.Log("Scene initialized: " + GameManager.Instance.GetAudioClip());
    }

    private void StartMusic()
    {
        musicManager.PlayMusic(GameManager.Instance.GetAudioClip());
    }
}
