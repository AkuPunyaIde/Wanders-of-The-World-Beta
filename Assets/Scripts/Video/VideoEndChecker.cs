using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndChecker : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private bool videoEnded = false;

    [Tooltip("Nama scene yang akan dimuat setelah video selesai dan dialog selesai")]
    public string nextSceneName;

    [Tooltip("DialogueActivator untuk memulai dialog setelah video selesai")]
    public DialogueActivator dialogueActivator;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += OnVideoEnd; // Attach event handler for when the video ends
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        videoEnded = true;

        // Mulai dialog setelah video selesai
        if (dialogueActivator != null)
        {
            dialogueActivator.InteractOnSceneLoad();
            dialogueActivator.OnDialogueComplete += LoadNextScene; // Pindah ke scene berikutnya setelah dialog selesai
        }
        else
        {
            Debug.LogWarning("DialogueActivator tidak ditemukan atau tidak diatur.");
            LoadNextScene(); // Langsung pindah ke scene berikutnya jika tidak ada dialog
        }
    }

    void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("Next scene name is not set!");
        }
    }
}
