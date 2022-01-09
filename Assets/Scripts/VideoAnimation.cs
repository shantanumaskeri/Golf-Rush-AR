using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoAnimation : MonoBehaviour
{

    public VideoPlayer videoPlayer;
    public GameManager gameManager;

    private IEnumerator Start()
    {
        while (!videoPlayer.isPlaying)
        {
            //Debug.Log("not playing");
            yield return null;
        }

        //Debug.Log("playing");

        for (int i = 0; i < gameManager.gameAssets.Length; i++)
        {
            gameManager.gameAssets[i].SetActive(false);
        }
    }

    public void PlayVideo(VideoClip vc)
    {
        videoPlayer.clip = vc;
        videoPlayer.Prepare();
        videoPlayer.Play();
        videoPlayer.loopPointReached += VideoPlaybackFinished;
    }

    private void VideoPlaybackFinished(VideoPlayer vp)
    {
        gameManager.EndGame();
    }

}
