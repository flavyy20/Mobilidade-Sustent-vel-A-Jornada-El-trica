using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class CutscenePlayer : MonoBehaviour
{
    public static CutscenePlayer Instance;

    [Header("Referências")]
    public GameObject cutsceneCanvas;
    public VideoPlayer videoPlayer;

    private UnityEvent onFinishEvent;

    void Awake()
    {
        Instance = this;
        if (cutsceneCanvas != null)
            cutsceneCanvas.SetActive(false);

        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    public void PlayCutscene(VideoClip clip, UnityEvent onFinish)
    {
        if (clip == null)
        {
            Debug.LogError("Cutscene sem vídeo!");
            return;
        }

        onFinishEvent = onFinish;

        cutsceneCanvas.SetActive(true);
        videoPlayer.clip = clip;

        videoPlayer.Play();
    }

    private void OnVideoFinished(VideoPlayer vp)
    {
        cutsceneCanvas.SetActive(false);

        if (onFinishEvent != null)
            onFinishEvent.Invoke();
    }
}
