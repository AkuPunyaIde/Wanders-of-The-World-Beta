using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SmashButtonGame : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private GameObject instructionBackground;
    [SerializeField] private float gameDuration = 10f;
    [SerializeField] private int requiredPresses = 20; // Jumlah tekan yang dibutuhkan untuk menang
    [SerializeField] private string winSceneName; // Nama scene yang akan dimuat jika menang
    [SerializeField] private string loseSceneName; // Nama scene yang akan dimuat jika kalah

    private float timeRemaining;
    private bool gameActive = false;
    private bool gameStarted = false;
    private int pressCount = 0;

    void Start()
    {
        timeRemaining = gameDuration;
        instructionText.gameObject.SetActive(true); // Tampilkan instruksi
        instructionBackground.SetActive(true); // Tampilkan latar belakang instruksi
        timerText.gameObject.SetActive(false); // Sembunyikan timer di awal
    }

    void Update()
    {
        if (!gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                StartGame();
            }
        }
        else if (gameActive)
        {
            timeRemaining -= Time.deltaTime;
            timerText.text = "Time: " + timeRemaining.ToString("F2");

            if (timeRemaining <= 0)
            {
                EndGame(false);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                pressCount++;
                if (pressCount >= requiredPresses)
                {
                    EndGame(true);
                }
            }
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        gameActive = true;
        instructionText.gameObject.SetActive(false); // Sembunyikan instruksi
        instructionBackground.SetActive(false); // Sembunyikan latar belakang instruksi
        timerText.gameObject.SetActive(true); // Tampilkan timer
        timeRemaining = gameDuration;
        pressCount = 0;
    }

    private void EndGame(bool success)
    {
        gameActive = false;
        GameData.Instance.GameResult = success;

        if (success)
        {
            SceneManager.LoadScene(winSceneName); // Muat scene yang ditentukan untuk kemenangan
        }
        else
        {
            SceneManager.LoadScene(loseSceneName); // Muat scene yang ditentukan untuk kekalahan
        }
    }
}
