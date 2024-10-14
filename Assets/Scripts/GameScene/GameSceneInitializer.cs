using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    [SerializeField] private SwordInteraction swordLeftRef;
    [SerializeField] private SwordInteraction swordRightRef;
    [SerializeField] private UIManager hudManagerRef;
 //   [SerializeField] private NoteSpawnManager noteSpawnManagerRef;
    [SerializeField] private NoteEndManager noteEndManagerRef;
    [SerializeField] private MusicManager musicManager;
    [SerializeField] private CubeNoteSpawnManager cubeNoteSpawnManagerRef;


    private void Start()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.SetGameSceneReferences(swordLeftRef, swordRightRef, hudManagerRef, noteEndManagerRef, musicManager);
            GameManager.Instance.SubcscribeGameSceneEvents();
            GameManager.Instance.ResetGameSceneStats();
            //noteSpawnManagerRef.BPM = GameManager.Instance.GetBpm();
            cubeNoteSpawnManagerRef.songData = GameManager.Instance.GetSongData();
            hudManagerRef.StartCountdown(StartMusic);           
        }
    }

    private void StartMusic()
    {
        musicManager.PlayMusic(GameManager.Instance.GetAudioClip());
    }
}
