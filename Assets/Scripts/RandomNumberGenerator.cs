using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomNumberGenerator : MonoBehaviour
{

    public GameManager gameManager;
    public XMLParser xmlParser;
    [HideInInspector]
    public List<int> inGameLevels;

    private List<int> allLevels;
    private GameObject application;

    public void Init()
    {
        application = GameObject.Find("Application");

        GenerateRandomLevels();
    }

    private void GenerateRandomLevels()
    {
        allLevels = new List<int>();
        inGameLevels = new List<int>();

        for (int i = 0; i < application.GetComponent<ApplicationSetup>().totalQuestions; i++)
        {
            allLevels.Add(i);
        }

        for (int i = 0; i < application.GetComponent<ApplicationSetup>().totalLevels; i++)
        {
            int randomNumber = allLevels[Random.Range(0, allLevels.Count)];
            inGameLevels.Add(randomNumber);

            allLevels.Remove(randomNumber);
        }

        /*for (int i = 0; i < application.GetComponent<ApplicationSetup>().totalLevels; i++)
        {
            Debug.Log(inGameLevels[i]);
        }*/

        if (SceneManager.GetActiveScene().name == "Level1")
        {
            gameManager.Init();
        }
        else
        {
            FindObjectOfType<ARGameManager>().Init();
        }
    }

}
