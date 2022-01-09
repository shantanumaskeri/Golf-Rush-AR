using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class LevelLoader : MonoBehaviour
{

    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    public Text loadingText;
    public VideoPlayer loadingBar;
    public Animator animator;

    private int levelIndex;
    private int sceneToLoadIndex;
    private GameObject music;
    private GameObject application;

    private void Start()
    {
        application = GameObject.Find("Application");

        music = GameObject.Find("Music");
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            music.GetComponent<AudioSource>().volume = 1.0f;
        }
        else
        {
            music.GetComponent<AudioSource>().volume = 0.3f;
        }
    }

    public void FadeToLevel(int index)
    {
        levelIndex = index;

        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        if (SceneManager.GetActiveScene().buildIndex < (SceneManager.sceneCountInBuildSettings - 1))
        {
            MoveToNextScene(levelIndex);
        }
        else
        {
            application.GetComponent<ApplicationSetup>().Reset();

            MoveToPreviousScene(levelIndex);
        }
    }

    private void MoveToNextScene(int index)
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + index);
    }

    private void MoveToPreviousScene(int index)
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex - index);
    }

    private void LoadLevel(int sceneIndex)
    {
        loadingScreen.SetActive(true);

        loadingBar.playbackSpeed = 1;
        loadingBar.loopPointReached += VideoPlaybackFinished;

        sceneToLoadIndex = sceneIndex;
        //StartCoroutine(LoadAsynchronously(sceneToLoadIndex));
    }

    private void VideoPlaybackFinished(VideoPlayer videoPlayer)
    {
        loadingBar.loopPointReached -= VideoPlaybackFinished;
        //Debug.Log("video finished");
        loadingText.text = "PLEASE WAIT WHILE SCENE FINISHES LOADING";
        StartCoroutine(LoadAsynchronously(sceneToLoadIndex));
    }

    private IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            slider.value = progress;
            
            if (progress < 1f)
            {
                progressText.text = progress * 100f + "%";
            }
            else
            {
                progressText.text = "LOADING. PLEASE WAIT...";

                operation.allowSceneActivation = true;
            }
            
            yield return null;
        }
    }

}
