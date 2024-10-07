using UnityEngine;

public class GameSceneInitializer : MonoBehaviour
{
    [SerializeField] private SwordInteraction swordLeftRef;
    [SerializeField] private SwordInteraction swordRightRef;
    [SerializeField] private UIManager hudManagerRef;
    [SerializeField] private NoteSpawnManager noteSpawnManagerRef;
    [SerializeField] private NoteEndManager noteEndManagerRef;
    [SerializeField] private MusicManager musicManager;

    private void Start()
    {
        if(GameManager.Instance != null)
        {
            GameManager.Instance.SetGameSceneReferences(swordLeftRef, swordRightRef, hudManagerRef, noteSpawnManagerRef, noteEndManagerRef, musicManager);
            GameManager.Instance.SubcscribeGameSceneEvents();
            GameManager.Instance.ResetGameSceneStats();
            musicManager.PlayMusic(GameManager.Instance.GetAudioClip());
            noteSpawnManagerRef.BPM = GameManager.Instance.GetBpm();           
        }
    }
}
