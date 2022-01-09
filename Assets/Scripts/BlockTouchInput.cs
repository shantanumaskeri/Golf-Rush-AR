using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BlockTouchInput : MonoBehaviour
{

	public GameManager gameManager;
	
	private void FixedUpdate()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
			{
				if (hit.collider.gameObject.tag == "QuizBlock")
				{
					hit.collider.gameObject.GetComponent<Animation>().CrossFade("Box001|Box click");
					hit.collider.gameObject.GetComponent<BoxCollider>().enabled = false;

					if (SceneManager.GetActiveScene().name == "Level1")
                    {
						gameManager.quizGrids[hit.collider.gameObject.GetComponent<GameBlocks>().id].SetActive(true);
						gameManager.quizAnimations[hit.collider.gameObject.GetComponent<GameBlocks>().id].SetActive(true);
						gameManager.quizAnimations[hit.collider.gameObject.GetComponent<GameBlocks>().id].GetComponent<DynamicSpriteAnimation>().enabled = true;
					}
					else
                    {
						GameObject.Find("Animation").GetComponent<Image>().enabled = true;
                    }
				}
			}
		}
	}

}
