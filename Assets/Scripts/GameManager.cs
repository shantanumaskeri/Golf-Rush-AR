using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{

	public XMLParser xmlParser;
	public RandomNumberGenerator randomNumberGenerator;
	public LevelLoader levelLoader;
	public VideoAnimation videoAnimation;

	public float transitionSpeed = 1.0f;
	public Camera mainCamera;
	public Transform currentView;
	public GameObject fixedJoystick;
	public GameObject jumpButton;
	public GameObject touchField;
	public GameObject feedbackCorrectPanel;
	public GameObject feedbackIncorrectPanel;
	public Text scoreText;
	//public Text timerText;
	public VideoClip videoClip1;
	public VideoClip videoClip2;
	public VideoClip videoClip3;
	public VideoClip videoClip4;
	public Text[] questionTexts;
	public Image[] answer1Imgs;
	public Image[] answer2Imgs;
	public Image[] answer3Imgs;
	public Image[] answer4Imgs;
	public Image[] progressCircles;
	public Text[] answer1Texts;
	public Text[] answer2Texts;
	public Text[] answer3Texts;
	public Text[] answer4Texts;
	public GameObject[] quizGrids;
	public GameObject[] quizAnimations;
	public GameObject[] questionBlocks;
	public GameObject[] gameAssets;

	//private float timer;
	private int questionsAnswered;
	private bool levelCompleted;
	//private bool levelStarted;
	private bool isAnimating;
	private bool canAnimate;
	private bool isAnswerSubmitted;
	private string question;
	private string answer1;
	private string answer2;
	private string answer3;
	private string answer4;
	private string correctAnswer;
	private GameObject application;
	//private int correctAnswers;

    /*private void Start()
    {
		correctAnswers = 0;
		Invoke("CheckCorrectAnswers", 2.0f);
    }*/

    public void Init()
    {
		application = GameObject.Find("Application");
		scoreText.text = "SCORE: " + application.GetComponent<ApplicationSetup>().gameScore;

		isAnswerSubmitted = false;
		levelCompleted = false;
		//levelStarted = false;
		isAnimating = false;
		canAnimate = true;
		questionsAnswered = 0;

		ResetQuiz();
		//ShowTimer();
    }

    /*private void Update()
    {
		if (!levelCompleted)
		{
			if (//levelStarted)
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

    private void LateUpdate()
    {
		if (Input.GetMouseButtonDown(0))
        {
			if (canAnimate)
            {
				if (!isAnimating)
				{
					isAnimating = true;
				}
			}
        }

		if (canAnimate)
        {
			if (isAnimating)
			{
				mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, currentView.position, Time.deltaTime * transitionSpeed);

				Vector3 currentAngle = new Vector3(Mathf.LerpAngle(mainCamera.transform.rotation.eulerAngles.x, currentView.transform.rotation.eulerAngles.x, Time.deltaTime * transitionSpeed), Mathf.LerpAngle(mainCamera.transform.rotation.eulerAngles.y, currentView.transform.rotation.eulerAngles.y, Time.deltaTime * transitionSpeed), Mathf.LerpAngle(mainCamera.transform.rotation.eulerAngles.z, currentView.transform.rotation.eulerAngles.z, Time.deltaTime * transitionSpeed));

				mainCamera.transform.eulerAngles = currentAngle;

				if (Vector3.Distance(mainCamera.transform.position, currentView.position) <= 0.01f)
                {
					mainCamera.transform.position = currentView.position;
					mainCamera.enabled = false;

					fixedJoystick.SetActive(true);
					jumpButton.SetActive(true);
					touchField.SetActive(true);

					isAnimating = false;
					canAnimate = false;
				}
			}
		}
    }

    public void StartGame()
    {
		//Debug.Log("StartGame");
		//levelStarted = true;
		CreateLevel();
    }

    private void CreateLevel()
	{
		ResetQuiz();
		
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("question", out question);
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("answer1", out answer1);
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("answer2", out answer2);
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("answer3", out answer3);
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("answer4", out answer4);
		xmlParser.gameQuizDictionary[randomNumberGenerator.inGameLevels[questionsAnswered]].TryGetValue("correctAnswer", out correctAnswer);

		questionTexts[questionsAnswered].text = question;
		answer1Texts[questionsAnswered].text = answer1;
		answer2Texts[questionsAnswered].text = answer2;
		answer3Texts[questionsAnswered].text = answer3;
		answer4Texts[questionsAnswered].text = answer4;

		answer1Imgs[questionsAnswered].color = answer2Imgs[questionsAnswered].color = answer3Imgs[questionsAnswered].color = answer4Imgs[questionsAnswered].color = Color.white;
		answer1Texts[questionsAnswered].color = answer2Texts[questionsAnswered].color = answer3Texts[questionsAnswered].color = answer4Texts[questionsAnswered].color = Color.white;// new Color(0.255f, 0.255f, 0.255f);

		AdjustLayoutByNumberOfAnswers(answer3Texts[questionsAnswered]);
		AdjustLayoutByNumberOfAnswers(answer4Texts[questionsAnswered]);
	}

	private void AdjustLayoutByNumberOfAnswers(Text answer)
	{
		if (answer.text == "")
		{
			answer.rectTransform.parent.gameObject.SetActive(false);
		}
		else
		{
			answer.rectTransform.parent.gameObject.SetActive(true);
		}
	}

	/*private void ShowTimer()
    {
		int minutes = Mathf.FloorToInt(//timer / 60F);
		int seconds = Mathf.FloorToInt(//timer - minutes * 60);
		string formattedTime = string.Format("{0:0}:{1:00}", minutes, seconds);

		//timerText.text = "TIME: " + formattedTime;
	}*/

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

		if (selectedAnswer.text == correctAnswer)
		{
			selectedAnswer.color = Color.white;
			selectedAnswer.rectTransform.parent.gameObject.GetComponent<Image>().color = Color.green;

			progressCircles[questionsAnswered].color = Color.green;

			levelCompleted = true;

			feedbackCorrectPanel.SetActive(true);

			application.GetComponent<ApplicationSetup>().numberOfCorrectAnswers++;
			
			IncrementScore();
		}
		else
		{
			selectedAnswer.color = Color.white;
			selectedAnswer.rectTransform.parent.gameObject.GetComponent<Image>().color = Color.red;

			progressCircles[questionsAnswered].color = Color.red;

			RevealCorrectAnswer(answer1Texts[questionsAnswered], correctAnswer);
			RevealCorrectAnswer(answer2Texts[questionsAnswered], correctAnswer);
			RevealCorrectAnswer(answer3Texts[questionsAnswered], correctAnswer);
			RevealCorrectAnswer(answer4Texts[questionsAnswered], correctAnswer);

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
		application.GetComponent<ApplicationSetup>().gameScore += 300;// Mathf.FloorToInt(//timer) * 20;
		scoreText.text = "SCORE: " + application.GetComponent<ApplicationSetup>().gameScore;
	}

    private void MoveToNextLevel()
    {
		questionsAnswered++;

		StartCoroutine(CloseCurrentQuiz(3.0f));
	}

	public void EndGame()
    {
		levelCompleted = true;
		//levelStarted = false;

		CheckCurrentScene();
	}

	private void CheckCurrentScene()
    {
		if (SceneManager.GetActiveScene().buildIndex == (SceneManager.sceneCountInBuildSettings - 1))
        {
			application.GetComponent<ApplicationSetup>().Reset();

			levelLoader.FadeToLevel(3);
		}
		else
        {
			application.GetComponent<ApplicationSetup>().level = 1;
			PlayerPrefs.SetInt("level", application.GetComponent<ApplicationSetup>().level);

			levelLoader.FadeToLevel(1);
		}
	}

	private IEnumerator CloseCurrentQuiz(float delay)
	{
		yield return new WaitForSeconds(delay);

		feedbackCorrectPanel.SetActive(false);
		feedbackIncorrectPanel.SetActive(false);
		
		//levelStarted = false;
		//timer = 60;

		//touchField.SetActive(true);
		quizGrids[questionsAnswered - 1].SetActive(false);
		questionBlocks[questionsAnswered - 1].transform.GetChild(0).gameObject.SetActive(false);
		questionBlocks[questionsAnswered - 1].GetComponent<BoxCollider>().enabled = false;

		if (questionsAnswered < application.GetComponent<ApplicationSetup>().totalLevels)
		{
			questionBlocks[questionsAnswered].GetComponent<BoxCollider>().enabled = true;
		}
		else
		{
			CheckCorrectAnswers();
			//EndGame();
		}

		isAnswerSubmitted = false;
		//ShowTimer();
	}

	private void CheckCorrectAnswers()
    {
		mainCamera.enabled = true;

		//Debug.Log("correct answers: " + application.GetComponent<ApplicationSetup>().numberOfCorrectAnswers);
		switch (application.GetComponent<ApplicationSetup>().numberOfCorrectAnswers)
        {
			case 0:
				videoAnimation.PlayVideo(videoClip1);
				break;

			case 1:
				videoAnimation.PlayVideo(videoClip2);
				break;

			case 2:
				videoAnimation.PlayVideo(videoClip3);
				break;

			case 3:
				videoAnimation.PlayVideo(videoClip4);
				break;
		}
    }

	private void ResetQuiz()
	{
		levelCompleted = false;

		//timer = application.GetComponent<ApplicationSetup>().levelTime;

		for (int i = 0; i < questionTexts.Length; i++)
		{
			questionTexts[i].text = answer1Texts[i].text = answer1Texts[i].text = answer3Texts[i].text = answer4Texts[i].text = "";
			answer1Imgs[i].color = answer2Imgs[i].color = answer3Imgs[i].color = answer4Imgs[i].color = Color.white;
		}
	}

}
