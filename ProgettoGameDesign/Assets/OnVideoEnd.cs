using UnityEngine;
using UnityEngine.Video;

public class OnVideoEnd : MonoBehaviour
{
    VideoPlayer video;
    [SerializeField] Canvas canvas;

    private void Awake()
    {
        Time.timeScale = 0f;
        video = GetComponent<VideoPlayer>();
    }

    private void OnEnable()
    {
        video.loopPointReached += VideoEnded;
    }

    private void OnDisable()
    {
        video.loopPointReached -= VideoEnded;
    }

    void VideoEnded(VideoPlayer vp)
    {
        Debug.Log("Video ended, resuming game and hiding canvas.");
        Time.timeScale = 1f;
        canvas.enabled = false;
    }
}
