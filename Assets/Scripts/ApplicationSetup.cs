using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationSetup : MonoBehaviour
{

    [HideInInspector]
    public int gameScore;
    [HideInInspector]
    public int totalQuestions;
    [HideInInspector]
    public int totalLevels;
    [HideInInspector]
    public int questionsAnswered;
    [HideInInspector]
    public int level;
    [HideInInspector]
    public int numberOfCorrectAnswers;
    [HideInInspector]
    public float levelTime;
    
    private void Start()
    {
        Init();
        Reset();
    }

    private void Update()
    {
        Quit();    
    }

    private void Init()
    {
        //PlayerPrefs.DeleteAll();

        DontDestroyOnLoad(GameObject.Find("Application"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Reset()
    {
        gameScore = 0;
        totalQuestions = 0;
        totalLevels = 0;
        questionsAnswered = 0;
        level = PlayerPrefs.GetInt("level");
        numberOfCorrectAnswers = 0;
        levelTime = 0.0f;
    }

    private void Quit()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

}
