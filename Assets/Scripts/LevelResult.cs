using UnityEngine;
using UnityEngine.UI;

public class LevelResult : MonoBehaviour
{

    public Text title;
    public Text body;

    private GameObject application;

    private void Start()
    {
        application = GameObject.Find("Application");

        ShowResult();
    }

    private void ShowResult()
    {
        switch (application.GetComponent<ApplicationSetup>().numberOfCorrectAnswers)
        {
            case 0:
                title.text = "Uh Oh!";
                break;

            case 1:
                title.text = "Nice Try!";
                break;

            case 2:
                title.text = "Well Done!";
                break;

            case 3:
                title.text = "Excellent!";
                break;
        }

        body.text = "You answered " + application.GetComponent<ApplicationSetup>().numberOfCorrectAnswers + "/3\n\nYou earned " + application.GetComponent<ApplicationSetup>().gameScore + " points";
    }
}
