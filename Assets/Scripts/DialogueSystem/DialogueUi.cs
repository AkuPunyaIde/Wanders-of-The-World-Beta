using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;

public class DialogueUi : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private Image faceExpressionImage;
    [SerializeField] private Image sceneImage;
    [SerializeField] private RawImage videoScreen;
    [SerializeField] private VideoPlayer videoPlayer;

    public bool IsOpen { get; private set; }

    private ResponseHandler responseHandler;
    private TypewritterEffect typeWritterEffect;

    public delegate void DialogueEnd();
    public event DialogueEnd OnDialogueEnd;

    private void Start()
    {
        // CloseDialogueBox();
    }

    private void Awake()
    {
        typeWritterEffect = GetComponent<TypewritterEffect>();
        responseHandler = GetComponent<ResponseHandler>();
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for (int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];

            // Update ekspresi wajah
            if (dialogueObject.FaceExpressions != null && dialogueObject.FaceExpressions.Length > i)
            {
                faceExpressionImage.sprite = dialogueObject.FaceExpressions[i];
                faceExpressionImage.gameObject.SetActive(true);
            }
            else
            {
                faceExpressionImage.gameObject.SetActive(false);
            }

            // Update gambar scene
            if (dialogueObject.SceneImages != null && dialogueObject.SceneImages.Length > i)
            {
                sceneImage.sprite = dialogueObject.SceneImages[i];
                sceneImage.gameObject.SetActive(true);
            }
            else
            {
                sceneImage.gameObject.SetActive(false);
            }

            // Update video scene
            if (dialogueObject.SceneVideos != null && dialogueObject.SceneVideos.Length > i)
            {
                videoPlayer.clip = dialogueObject.SceneVideos[i];
                videoPlayer.Play();
                videoScreen.texture = videoPlayer.texture;
                videoScreen.gameObject.SetActive(true);
            }
            else
            {
                videoPlayer.Stop();
                videoScreen.gameObject.SetActive(false);
            }

            yield return RunTypingEffect(dialogue);

            textLabel.text = dialogue;

            if (i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses) break;

            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        typeWritterEffect.Run(dialogue, textLabel);

        while (typeWritterEffect.IsRunning)
        {
            yield return null;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                typeWritterEffect.Stop();
            }
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        if (textLabel != null)
        {
            textLabel.text = string.Empty;
        }

        faceExpressionImage.gameObject.SetActive(false);
        sceneImage.gameObject.SetActive(false);
        videoPlayer.Stop();
        videoScreen.gameObject.SetActive(false);

        OnDialogueEnd?.Invoke();
    }
}
