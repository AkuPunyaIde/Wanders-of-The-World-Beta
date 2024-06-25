using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public int totalItemsToCollect = 3;
    private int collectedItemCount = 0;

    public UnityEvent onAllItemsCollected;  // Event to be triggered when all items are collected
    public GameObject targetObject;  // The object to be activated

    public UnityEvent startEvent;

    private void Start()
    {
        startEvent.Invoke();

        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
        if (GameData.Instance != null)
        {
            collectedItemCount = GameData.Instance.CollectedItemCount;
        }
        else
        {
            Debug.LogWarning("GameData.Instance not found.");
        }
    }

    public void CollectItem()
    {
        collectedItemCount++;
        if (GameData.Instance != null)
        {
            GameData.Instance.CollectedItemCount = collectedItemCount;
        }
        else
        {
            Debug.LogWarning("GameData.Instance not found.");
        }

        if (collectedItemCount >= totalItemsToCollect)
        {
            onAllItemsCollected.Invoke();
        }
    }

    private void OnEnable()
    {
        onAllItemsCollected.AddListener(ActivateTargetObject);
    }

    private void OnDisable()
    {
        onAllItemsCollected.RemoveListener(ActivateTargetObject);
    }

    private void ActivateTargetObject()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }

    public void LoadMinigameScene()
    {
        SceneManager.LoadScene("MinigameSceneName"); // Replace "MinigameSceneName" with the actual name of your minigame scene
    }

    public void HandleGameResult()
    {
        if (GameData.Instance != null && GameData.Instance.GameResult)
        {
            // Logic for winning the game
            SceneManager.LoadScene("NextSceneName"); // Replace "NextSceneName" with the actual name of the next scene
        }
        else
        {
            // Logic for losing the game
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
