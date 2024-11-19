// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;

// public class AnswerScript : MonoBehaviour
// {
//     public bool isCorrect = false;

//     public QuizManager quizManager;

//     public void Answer()
//     {
//         if(isCorrect)
//         {   
//             Debug.Log("Correct Answer");
            
//             quizManager.correct();
//         }
//         else
//         {
//             Debug.Log("Wrong Answer");
            
//             quizManager.wrong();
//         }
//     }


// }

using UnityEngine;
using UnityEngine.UI;

public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false;
    public QuizManager quizManager;

    public void Answer()
    {
        if (isCorrect)
        {
            Debug.Log("Correct Answer");
            quizManager.correct();
            ChangeColor(Color.yellow); // Change to yellow for correct answer
        }
        else
        {
            Debug.Log("Wrong Answer");
            quizManager.wrong();
            ChangeColor(Color.red); // Change to red for wrong answer
        }
    }

    private void ChangeColor(Color color)
    {
        Image buttonImage = GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.color = color; // Set the color of the button
        }
    }
}
