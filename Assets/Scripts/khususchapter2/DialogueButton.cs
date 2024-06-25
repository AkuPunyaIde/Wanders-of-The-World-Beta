using UnityEngine;
using UnityEngine.UI;

public class DialogueButton : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueUi dialogueUI;
    [SerializeField] private CanvasActivator canvasActivator;

    private void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(OnButtonClick);
        }
    }

    private void OnButtonClick()
    {
        if (dialogueUI != null && dialogueObject != null)
        {
            dialogueUI.ShowDialogue(dialogueObject);

            // Tutup Canvas setelah memunculkan dialog
            if (canvasActivator != null)
            {
                canvasActivator.CloseCanvas();
            }
        }
    }
}
