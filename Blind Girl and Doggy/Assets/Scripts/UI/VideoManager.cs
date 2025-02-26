using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private float times;
    [SerializeField] private bool ending;
    private int secret;

    void Awake()
    {
        secret = PlayerPrefs.GetInt("UnlockSecret");
        videoPlayer.playOnAwake = false;
        videoPlayer.isLooping = false;
    }

    void Start()
    {
        videoPlayer.prepareCompleted += OnVideoPrepared;
        videoPlayer.Prepare();
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video is ready to play!");
        StartCoroutine(PlayVideoWithDelay(0.5f));
    }

    IEnumerator PlayVideoWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        videoPlayer.Play();
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Video finished playing!");
        StartCoroutine(NextScene(times));
    }

    IEnumerator NextScene(float time)
    {
        yield return new WaitForSeconds(time);

        if (secret > 0 && ending)
        {
            PlayerPrefs.SetInt("UnlockSecret", 0);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Secret");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }
    }
}

