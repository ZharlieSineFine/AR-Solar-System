using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class QuizManager2 : MonoBehaviour
{
   public List<QuestionAndAnswers2> QnA;
   public GameObject[] options;
   public int currentQuestion;

   public TMP_Text QuestionTxt;
   public TMP_Text ScoreTxt;

   int totalQuestions = 0;
   public int score;

   public GameObject QuizPanel;
   public GameObject GoPanel;


   private void Start()
   {
        totalQuestions = QnA.Count;
        GoPanel.SetActive(false);
        generateQuestion();

   }

   public void retry()
   {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
   }


    public void GameOver()
    {   
        QuizPanel.SetActive(false);
        GoPanel.SetActive(true);
        ScoreTxt.text= score + "/" + totalQuestions;
    }


   public void correct()
   {
    if (currentQuestion >= 0 && currentQuestion < QnA.Count)
        {
            score += 1;
            QnA.RemoveAt(currentQuestion);
            StartCoroutine(WaitAndResetColors(1f));
            // generateQuestion();
        }
        else
        {
            Debug.LogWarning("Attempted to remove an invalid question index.");
        }
   }

   public void wrong()
   {
    //when you answer wrong
    if (currentQuestion >= 0 && currentQuestion < QnA.Count)
        {   
            QnA.RemoveAt(currentQuestion);
            StartCoroutine(WaitAndResetColors(1f));
            // generateQuestion();
        }
        else
        {
            Debug.LogWarning("Attempted to remove an invalid question index.");
        }
   }

   void SetAnswers()
   {
    for (int i = 0; i < options.Length; i++)
    {
        options[i].GetComponent<AnswerScript2>().isCorrect = false;
        options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];
        if(QnA[currentQuestion].CorrectAnswer == i+1)
        {
            options[i].GetComponent<AnswerScript2>().isCorrect = true;
        }
    }
   }

    public void ResetButtonColors()
    {
        foreach (GameObject option in options)
        {
            Image buttonImage = option.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.color = Color.black; 
            }
        }
    }

    IEnumerator WaitAndResetColors(float waitTime)
    {
    yield return new WaitForSeconds(waitTime); // Wait for the specified time
    ResetButtonColors(); // Reset button colors after the wait
    generateQuestion();
    }

   void generateQuestion()
   {
        if(QnA.Count > 0)
        {
            currentQuestion = Random.Range(0, QnA.Count);
            QuestionTxt.text = QnA[currentQuestion].Question;
            SetAnswers();
        }
        else{
            Debug.Log("Out of Questions");
            GameOver();
        }
        
   }
}