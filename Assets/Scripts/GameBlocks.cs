using UnityEngine;
using UnityEngine.UI;

public class GameBlocks : MonoBehaviour
{

    public int id;

    public void SubmitAnswer(Text answerText)
    {
        FindObjectOfType<ARGameManager>().SubmitQuizAnswer(answerText);
    }

}
