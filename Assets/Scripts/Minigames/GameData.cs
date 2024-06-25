using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData Instance { get; private set; }

    private int collectedItemCount = 0;
    private bool gameResult;

    public int CollectedItemCount
    {
        get { return collectedItemCount; }
        set { collectedItemCount = value; }
    }

    public bool GameResult
    {
        get { return gameResult; }
        set { gameResult = value; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        collectedItemCount = 0; // Initialize collectedItemCount to 0 at the start
        gameResult = false; // Initialize gameResult to false
    }
}
