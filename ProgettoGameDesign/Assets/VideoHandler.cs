using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    [SerializeField] protected InputActionReference _cancelActionReference;
    [Header("Video")]
    [SerializeField] protected VideoPlayer videoPlayer;
    [SerializeField] protected ScenesToLoad gameStart;
    private void OnEnable()
    {
        videoPlayer.loopPointReached += VideoEnded;
        _cancelActionReference.action.performed += SkipVideo;
    }
    private void OnDisable()
    {
        videoPlayer.loopPointReached -= VideoEnded;
        _cancelActionReference.action.performed -= SkipVideo;
    }

    private void SkipVideo(InputAction.CallbackContext context)
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Stop();
            SceneController.Instance.LoadSceneSet(gameStart);
        }

    }
    void VideoEnded(VideoPlayer vp)
    {
        SceneController.Instance.LoadSceneSet(gameStart);
    }
}
