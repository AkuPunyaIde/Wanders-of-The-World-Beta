using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public Slider volumeSlider;
    public Button muteButton;
    private bool isMuted = false;
    private float previousVolume;

    // List of scenes where audio should stop
    public string[] scenesToStopAudio;

    // List of scenes and corresponding audio clips to play
    [System.Serializable]
    public class SceneAudio
    {
        public string sceneName;
        public AudioClip audioClip;
    }

    public SceneAudio[] sceneAudioList;
    private string currentSceneName = "";

    void Awake()
    {
        // Check if there is already an instance of AudioManager
        if (FindObjectsOfType<AudioManager>().Length > 1)
        {
            Destroy(gameObject); // Destroy duplicate
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Keep this instance
            SceneManager.sceneLoaded += OnSceneLoaded; // Add listener for scene loaded event
        }

        // Load saved settings
        float savedVolume = PlayerPrefs.GetFloat("Volume", 1f);  // Default volume is 1
        isMuted = PlayerPrefs.GetInt("Muted", 0) == 1;  // Default is not muted

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }

        if (muteButton != null)
        {
            muteButton.onClick.AddListener(ToggleMute);
        }

        audioSource.volume = isMuted ? 0 : savedVolume;
    }

    void Start()
    {
        PlaySceneAudio(SceneManager.GetActiveScene().name);
    }

    public void SetVolume(float volume)
    {
        if (!isMuted)
        {
            audioSource.volume = volume;
        }
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            previousVolume = audioSource.volume;
            audioSource.volume = 0;
        }
        else
        {
            audioSource.volume = previousVolume;
        }
        PlayerPrefs.SetInt("Muted", isMuted ? 1 : 0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        foreach (string sceneName in scenesToStopAudio)
        {
            if (scene.name == sceneName)
            {
                audioSource.Stop();
                return;
            }
        }

        PlaySceneAudio(scene.name);
    }

    private void PlaySceneAudio(string sceneName)
    {
        if (currentSceneName == sceneName && audioSource.isPlaying)
        {
            // Audio is already playing for this scene, do nothing
            return;
        }

        foreach (SceneAudio sceneAudio in sceneAudioList)
        {
            if (sceneAudio.sceneName == sceneName)
            {
                audioSource.clip = sceneAudio.audioClip;
                audioSource.Play();
                break;
            }
        }

        currentSceneName = sceneName;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Remove listener when destroyed
    }
}
