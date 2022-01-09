using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARGameManager : MonoBehaviour
{

	public XMLParser xmlParser;
	public RandomNumberGenerator randomNumberGenerator;
	public LevelLoader levelLoader;
	
	public GameObject feedbackCorrectPanel;
	public GameObject feedbackIncorrectPanel;
	public Text scoreText;
	//public Text timerText;
	public Image[] progressCircles;
	
	[HideInInspector]
	public List<GameObject> quizGridList = new List<GameObject>();
	[HideInInspector]
	public bool isQuizInPlay = false;

	//private float timer;
	private int questionsAnswered;
	private bool levelCompleted;
	//private bool levelStarted;
	private bool isAnswerSubmitted;
	private string question;
	private string answer1;
	private string answer2;
	private string answer3;
	private string answer4;
	private string correctAnswer;
	private GameObject application;
	
	public void Init()
    {
		application = GameObject.Find("Application");
		scoreText.text = "SCORE: " + application.GetComponent<ApplicationSetup>().gameScore;

		isAnswerSubmitted = false;
		levelCompleted = false;
		questionsAnswered = 0;

		ResetQuiz();
		//ShowTimer();
    }

    /*private void Update()
    {
		if (!levelCompleted)
		{
			if (levelStarted)
            {
				//timer -= Time.deltaTime;

				if (//timer < 0.0f)
				{
					//timer = 0.0f;

					levelCompleted = true;
					//levelStarted = false;

					MoveToNextLevel();
				}

				ShowTimer();
			}
		}
    }*/

    public void StartGame()
    {
		Debug.Log("StartGame");
		
		CreateLevel();
    }

    private void CreateLevel()
	{
		Debug.Log("CreateLevel");

		isQuizInPlay = true;

		ResetQuiz();
		
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("question", out question);
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("answer1", out answer1);
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("answer2", out answer2);
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("answer3", out answer3);
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("answer4", out answer4);
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("correctAnswer", out correctAnswer);

		Debug.Log("CreateLevel " + correctAnswer + " : " + questionsAnswered);
		GameObject.Find("Question").GetComponent<Text>().text = question;
		GameObject.Find("Answer1Text").GetComponent<Text>().text = answer1;
		GameObject.Find("Answer2Text").GetComponent<Text>().text = answer2;
		GameObject.Find("Answer3Text").GetComponent<Text>().text = answer3;
		GameObject.Find("Answer4Text").GetComponent<Text>().text = answer4;

		GameObject.Find("BtnAnswer1").GetComponent<Image>().color = GameObject.Find("BtnAnswer2").GetComponent<Image>().color = GameObject.Find("BtnAnswer3").GetComponent<Image>().color = GameObject.Find("BtnAnswer4").GetComponent<Image>().color = Color.white;

		//GameObject.Find("Answer1Text").GetComponent<Text>().color = GameObject.Find("Answer1Text").GetComponent<Text>().color = GameObject.Find("Answer1Text").GetComponent<Text>().color = GameObject.Find("Answer1Text").GetComponent<Text>().color = Color.white;
	}

	public void SubmitQuizAnswer(Text selectedAnswer)
	{
		if (!levelCompleted)
        {
			if (!isAnswerSubmitted)
            {
				selectedAnswer.rectTransform.parent.gameObject.GetComponent<Image>().color = Color.yellow;

				StartCoroutine(CheckAnswerSubmitted(2.0f, selectedAnswer));

				isAnswerSubmitted = true;
			}
		}	
	}

	private IEnumerator CheckAnswerSubmitted(float delay, Text selectedAnswer)
    {
		yield return new WaitForSeconds(delay);

		Debug.Log("CheckAnswerSubmitted " + selectedAnswer.text + " == " + correctAnswer);
		if (selectedAnswer.text == correctAnswer)
		{
			//selectedAnswer.color = Color.white;
			selectedAnswer.rectTransform.parent.gameObject.GetComponent<Image>().color = Color.green;

			progressCircles[questionsAnswered].color = Color.green;

			levelCompleted = true;

			feedbackCorrectPanel.SetActive(true);

			application.GetComponent<ApplicationSetup>().numberOfCorrectAnswers++;
			
			IncrementScore();
		}
		else
		{
			//selectedAnswer.color = Color.white;
			selectedAnswer.rectTransform.parent.gameObject.GetComponent<Image>().color = Color.red;

			progressCircles[questionsAnswered].color = Color.red;

			RevealCorrectAnswer(GameObject.Find("Answer1Text").GetComponent<Text>(), correctAnswer);
			RevealCorrectAnswer(GameObject.Find("Answer2Text").GetComponent<Text>(), correctAnswer);
			RevealCorrectAnswer(GameObject.Find("Answer3Text").GetComponent<Text>(), correctAnswer);
			RevealCorrectAnswer(GameObject.Find("Answer4Text").GetComponent<Text>(), correctAnswer);

			levelCompleted = true;

			feedbackIncorrectPanel.SetActive(true);
		}

		MoveToNextLevel();
	}

	private void RevealCorrectAnswer(Text answerText, string answer)
	{
		if (answerText.text == answer)
		{
			answerText.color = Color.white;
			answerText.rectTransform.parent.gameObject.GetComponent<Image>().color = Color.green;
		}
	}

	private void IncrementScore()
    {
		application.GetComponent<ApplicationSetup>().gameScore += 300;// Mathf.FloorToInt(timer) * 20;
		scoreText.text = "SCORE: " + application.GetComponent<ApplicationSetup>().gameScore;
	}

    private void MoveToNextLevel()
    {
		questionsAnswered++;

		StartCoroutine(CloseCurrentQuiz(3.0f));

		if (questionsAnswered < application.GetComponent<ApplicationSetup>().totalLevels)
        {
			//questionBlocks[questionsAnswered].GetComponent<BoxCollider>().enabled = true;
		}
		else
		{
			StartCoroutine(EndGame(3.0f));
		}
	}

	private IEnumerator EndGame(float delay)
    {
		yield return new WaitForSeconds(delay);
		
		levelCompleted = true;
		//levelStarted = false;

		CheckCurrentScene();
	}

	private void CheckCurrentScene()
    {
		/*if (SceneManager.GetActiveScene().buildIndex == (SceneManager.sceneCountInBuildSettings - 1))
        {
			application.GetComponent<ApplicationSetup>().Reset();

			levelLoader.FadeToLevel(3);
		}
		else
        {
			application.GetComponent<ApplicationSetup>().level = 1;
			PlayerPrefs.SetInt("level", application.GetComponent<ApplicationSetup>().level);

			
		}*/

		levelLoader.FadeToLevel(1);
	}

	private IEnumerator CloseCurrentQuiz(float delay)
	{
		yield return new WaitForSeconds(delay);

		feedbackCorrectPanel.SetActive(false);
		feedbackIncorrectPanel.SetActive(false);
		
		//levelStarted = false;
		//timer = 60;

		//quizGrids[questionsAnswered - 1].SetActive(false);
		//questionBlocks[questionsAnswered - 1].transform.GetChild(0).gameObject.SetActive(false);
		//questionBlocks[questionsAnswered - 1].GetComponent<BoxCollider>().enabled = false;
		quizGridList[questionsAnswered - 1].SetActive(false);

		if (questionsAnswered < application.GetComponent<ApplicationSetup>().totalLevels)
		{
			isQuizInPlay = false;
			isAnswerSubmitted = false;
		}

		//ShowTimer();
	}

	private void ResetQuiz()
	{
		levelCompleted = false;

		//timer = application.GetComponent<ApplicationSetup>().levelTime;

		if (GameObject.Find("Question") == null)
			return;

		GameObject.Find("Question").GetComponent<Text>().text = GameObject.Find("Answer1Text").GetComponent<Text>().text = GameObject.Find("Answer2Text").GetComponent<Text>().text = GameObject.Find("Answer3Text").GetComponent<Text>().text = GameObject.Find("Answer4Text").GetComponent<Text>().text = "";

		GameObject.Find("BtnAnswer1").GetComponent<Image>().color = GameObject.Find("BtnAnswer2").GetComponent<Image>().color = GameObject.Find("BtnAnswer3").GetComponent<Image>().color = GameObject.Find("BtnAnswer4").GetComponent<Image>().color = Color.white;
	}

}
