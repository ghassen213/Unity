using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMusicManager : MonoBehaviour
{
    private static GameMusicManager instance = null;
    
    [Header("Music Tracks")]
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;
    
    private AudioSource audioSource;
    private string currentSceneName;

    public static GameMusicManager Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this) 
        {
            Destroy(this.gameObject);
            return;
        } 
        else 
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
        
        // Add AudioSource component if not exists
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
            
        // Configure AudioSource for music
        audioSource.loop = true;
        audioSource.volume = 0.3f;
        
        // Subscribe to scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void Start()
    {
        // Play appropriate music for starting scene
        CheckSceneMusic(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CheckSceneMusic(scene.name);
    }
    
    private void CheckSceneMusic(string sceneName)
    {
        AudioClip targetMusic = null;
        
        // Determine which music to play based on scene
        if (sceneName == "MainMenu" || sceneName.Contains("Menu"))
        {
            targetMusic = mainMenuMusic;
        }
        else if (sceneName == "Gameplay" || sceneName == "Level1" || sceneName.Contains("Level"))
        {
            targetMusic = gameplayMusic;
        }
        
        // Change music if needed
        if (targetMusic != null && audioSource.clip != targetMusic)
        {
            PlayMusic(targetMusic);
        }
    }
    
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        
        audioSource.clip = clip;
        audioSource.Play();
    }
    
    public void StopMusic()
    {
        audioSource.Stop();
    }
    
    public void SetVolume(float volume)
    {
        audioSource.volume = Mathf.Clamp01(volume);
    }
    
    // Clean up when destroyed
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}