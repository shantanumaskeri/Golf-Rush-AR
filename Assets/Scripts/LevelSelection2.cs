using UnityEngine;
using UnityEngine.UI;

public class LevelSelection2 : MonoBehaviour
{

    public Button arCore;
    public GameObject lockPatch;

    private GameObject application;

    private void Start()
    {
        application = GameObject.Find("Application");

        CheckLevelStatus();
    }

    private void CheckLevelStatus()
    {
        arCore.interactable = false;// System.Convert.ToBoolean(application.GetComponent<ApplicationSetup>().level);
        lockPatch.SetActive(true);//!System.Convert.ToBoolean(application.GetComponent<ApplicationSetup>().level));
    }

}
