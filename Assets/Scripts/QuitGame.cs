using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void Quit()
    {
#if UNITY_EDITOR
        // Jika menjalankan game di Unity Editor, hentikan mode play
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Jika menjalankan build game, keluar dari aplikasi
        Application.Quit();
#endif
    }
}
