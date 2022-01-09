using UnityEngine;
using UnityEngine.UI;

public class StaticSpriteAnimation : MonoBehaviour
{

	public Sprite[] frames;

    private int index = 0;
	private readonly int framesPerSecond = 30;

	private void Update()
	{
		//index += 1;
		index = (int)(Time.timeSinceLevelLoad * framesPerSecond);

		if (index >= 0 && index < frames.Length)
		{
			index = index % frames.Length;

			GetComponent<Image>().sprite = frames[index];
		}
	}

}
