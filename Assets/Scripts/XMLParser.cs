using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class XMLParser : MonoBehaviour
{

	public RandomNumberGenerator randomNumberGenerator;
	public TextAsset inFile;

    [HideInInspector]
	public List<Dictionary<string,string>> gameQuizDictionary;
	
	private Dictionary<string,string> quizDetails;
	private GameObject application;

	private void Start()
	{
		gameQuizDictionary = new List<Dictionary<string, string>>();
		
		application = GameObject.Find("Application");

		LoadXML();
	}

	private void LoadXML()
    {
		XmlDocument xmlDoc = new XmlDocument();
		xmlDoc.LoadXml(inFile.text);

		ParseXML(xmlDoc);
	}

	private void ParseXML(XmlDocument document)
	{
		XmlNodeList questionnaireList = document.GetElementsByTagName("questionnaire");
	
		foreach (XmlNode questionnaireInfo in questionnaireList)
		{
			XmlNodeList quiz = questionnaireInfo.ChildNodes;
			quizDetails = new Dictionary<string, string>();

			foreach (XmlNode quizItems in quiz)
			{
				quizDetails.Add(quizItems.Name, quizItems.InnerText);
			}

			gameQuizDictionary.Add(quizDetails);
		}

		application.GetComponent<ApplicationSetup>().totalQuestions = application.GetComponent<ApplicationSetup>().totalLevels = gameQuizDictionary.Count;
		
		XmlNodeList levelList = document.GetElementsByTagName("levels");
		foreach (XmlNode levelInfo in levelList)
		{
			XmlNodeList totalLevels = levelInfo.ChildNodes;

			foreach (XmlNode levelItem in totalLevels)
			{
				application.GetComponent<ApplicationSetup>().totalLevels = int.Parse(levelItem.InnerText);
			}
		}

		XmlNodeList timeList = document.GetElementsByTagName("time");
		foreach (XmlNode timeInfo in timeList)
		{
			XmlNodeList totalTime = timeInfo.ChildNodes;

			foreach (XmlNode timeItem in totalTime)
			{
				application.GetComponent<ApplicationSetup>().levelTime = float.Parse(timeItem.InnerText);
			}
		}

		randomNumberGenerator.Init();
	}

}