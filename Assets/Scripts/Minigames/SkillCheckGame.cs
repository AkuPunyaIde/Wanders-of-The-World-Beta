using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SkillCheckGame : MonoBehaviour
{
    [SerializeField] private Image skillCheckSlider;
    [SerializeField] private Image targetArea;
    [SerializeField] private float sliderSpeed = 200f; // Speed of the slider movement
    [SerializeField] private string sceneToLoad; // Name of the scene to load after the game ends
    [SerializeField] private TMP_Text instructionText;
    [SerializeField] private TMP_Text resultText;

    private bool gameStarted = false;
    private bool skillCheckActive = false;
    private float sliderDirection = 1f; // Direction of the slider movement
    private RectTransform skillCheckSliderRect;

    private void Start()
    {
        instructionText.text = "Press E to Start";
        resultText.gameObject.SetActive(false);
        skillCheckSliderRect = skillCheckSlider.GetComponent<RectTransform>();
        skillCheckSliderRect.anchoredPosition = new Vector2(-skillCheckSliderRect.rect.width / 2, skillCheckSliderRect.anchoredPosition.y);
    }

    private void Update()
    {
        if (!gameStarted && Input.GetKeyDown(KeyCode.E))
        {
            StartGame();
        }

        if (gameStarted && skillCheckActive)
        {
            MoveSlider();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CheckSkill();
            }
        }
    }

    private void StartGame()
    {
        gameStarted = true;
        instructionText.gameObject.SetActive(false);
        skillCheckActive = true;
        skillCheckSliderRect.anchoredPosition = new Vector2(-skillCheckSliderRect.rect.width / 2, skillCheckSliderRect.anchoredPosition.y);
    }

    private void MoveSlider()
    {
        skillCheckSliderRect.anchoredPosition += new Vector2(sliderSpeed * sliderDirection * Time.deltaTime, 0);

        if (skillCheckSliderRect.anchoredPosition.x >= skillCheckSliderRect.rect.width / 2 || skillCheckSliderRect.anchoredPosition.x <= -skillCheckSliderRect.rect.width / 2)
        {
            sliderDirection *= -1f;
        }
    }

    private void CheckSkill()
    {
        float sliderPosition = skillCheckSliderRect.anchoredPosition.x;
        float targetMin = targetArea.rectTransform.anchoredPosition.x - targetArea.rectTransform.rect.width / 2;
        float targetMax = targetArea.rectTransform.anchoredPosition.x + targetArea.rectTransform.rect.width / 2;

        if (sliderPosition >= targetMin && sliderPosition <= targetMax)
        {
            EndGame(true);
        }
        else
        {
            EndGame(false);
        }
    }

    private void EndGame(bool success)
    {
        skillCheckActive = false;
        resultText.gameObject.SetActive(true);
        resultText.text = success ? "Success!" : "Failed!";
        GameData.Instance.GameResult = success;
        Invoke("LoadNextScene", 2f); // Load the next scene after a delay
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
