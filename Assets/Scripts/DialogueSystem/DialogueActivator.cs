using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueActivator : MonoBehaviour, DialogueInteract
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private bool startDialogueOnTriggerEnter = false;  // Flag to start dialogue on trigger enter
    public bool disableAfterUse = true;  // Flag to determine if the dialogue should be disabled after use

    public delegate void DialogueComplete();
    public event DialogueComplete OnDialogueComplete;

    private bool hasTriggeredDialogue = false;
    private bool dialogueEnded = false;  // Flag to check if dialogue has ended

    private void Start()
    {
        // Uncomment this line if you want to start dialogue when the scene loads
        // InteractOnSceneLoad();
    }

    public void UpdateDialogueObject(DialogueObject dialogueObject)
    {
        this.dialogueObject = dialogueObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            player.Interact = this;
            if (startDialogueOnTriggerEnter && !hasTriggeredDialogue)
            {
                Interact(player);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            if (player.Interact is DialogueActivator dialogueActivator && dialogueActivator == this)
            {
                player.Interact = null;
            }
        }
    }

    public void Interact(PlayerMovement player)
    {
        if (startDialogueOnTriggerEnter && hasTriggeredDialogue) return;

        foreach (DialogueResponseEvents responseEvents in GetComponents<DialogueResponseEvents>())
        {
            if (responseEvents.DialogueObject == dialogueObject)
            {
                player.DialogueUi.AddResponseEvents(responseEvents.Events);
                break;
            }
        }

        player.DialogueUi.ShowDialogue(dialogueObject);
        player.DialogueUi.OnDialogueEnd += HandleDialogueEnd;

        if (startDialogueOnTriggerEnter)
        {
            hasTriggeredDialogue = true;
        }
    }

    private void HandleDialogueEnd()
    {
        if (dialogueEnded) return;  // Prevent multiple calls

        dialogueEnded = true;

        if (disableAfterUse)
        {
            gameObject.SetActive(false);  // Disable the game object after dialogue ends
        }

        // Log to ensure this method is called
        Debug.Log("Dialogue ended. Checking GameManager to collect item.");

        // Logika untuk menghitung item yang dikumpulkan
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.CollectItem();
        }
        else
        {
            Debug.LogWarning("GameManager not found in the scene.");
        }

        // Call the ChangeScene method to change the scene
        ChangeScene();

        // Ensure event listener is removed after dialogue ends
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            player.DialogueUi.OnDialogueEnd -= HandleDialogueEnd;
        }
    }

    public void InteractOnSceneLoad()
    {
        PlayerMovement player = FindObjectOfType<PlayerMovement>();
        if (player != null)
        {
            Interact(player);
        }
        else
        {
            Debug.LogWarning("PlayerMovement tidak ditemukan di scene.");
        }
    }

    // Method untuk mengganti scene jika diperlukan
    public void ChangeScene()
    {
        if (dialogueObject != null && !string.IsNullOrEmpty(dialogueObject.NextSceneName))
        {
            Debug.Log("Changing scene to: " + dialogueObject.NextSceneName);
            SceneManager.LoadScene(dialogueObject.NextSceneName);
        }
        else
        {
            Debug.LogWarning("Nama scene tujuan tidak diatur di DialogueObject atau kosong.");
        }
    }
}
