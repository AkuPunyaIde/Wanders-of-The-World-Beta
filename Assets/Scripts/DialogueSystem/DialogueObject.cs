using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField][TextArea] private string[] dialogue;
    [SerializeField] private Response[] responses;
    [SerializeField] private Sprite[] faceExpressions;
    [SerializeField] private Sprite[] sceneImages;
    [SerializeField] private VideoClip[] sceneVideos;
    [SerializeField] private string nextSceneName;  // Tambahkan properti ini

    public string[] Dialogue => dialogue;
    public Response[] Responses => responses;
    public Sprite[] FaceExpressions => faceExpressions;
    public Sprite[] SceneImages => sceneImages;
    public VideoClip[] SceneVideos => sceneVideos;
    public string NextSceneName => nextSceneName;  // Tambahkan getter ini

    public bool HasResponses => Responses != null && Responses.Length > 0;
}