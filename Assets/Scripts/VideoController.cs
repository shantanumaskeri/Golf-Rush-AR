using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{

    public Text continueText;
    public VideoPlayer videoPlayer;

    private void Start()
    {
        videoPlayer.loopPointReached += VideoFinishedPlaying;
    }

    private void VideoFinishedPlaying(VideoPlayer vp)
    {
        continueText.text = "Tap to Continue";

        vp.loopPointReached -= VideoFinishedPlaying;
    }

}
