using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DynamicSpriteAnimation : MonoBehaviour
{

	public GameObject quizPanel;
	public GameManager gameManager;
	public Sprite[] frames;

    private int index = 0;
	
	private void Update()
	{
		if (GetComponent<Image>().enabled)
        {
			index += 1; //(int)(Time.timeSinceLevelLoad * framesPerSecond);

			if (index >= 0 && index < frames.Length)
			{
				index = index % frames.Length;

				GetComponent<Image>().sprite = frames[index];
			}
			else
			{
				quizPanel.SetActive(true);

				if (SceneManager.GetActiveScene().name == "Level2")
				{
					FindObjectOfType<ARGameManager>().StartGame();
				}
				else
				{
					gameManager.StartGame();
				}

				gameObject.SetActive(false);
			}
		}
	}

}
