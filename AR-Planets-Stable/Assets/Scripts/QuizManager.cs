// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using TMPro; 
// using UnityEngine.SceneManagement;

// public class QuizManager : MonoBehaviour
// {
//    public List<QuestionAndAnswer> QnA;
//    public GameObject[] options;
//    public int currentQuestion;

//    public TMP_Text QuestionTxt;
//    public TMP_Text ScoreTxt;

//    int totalQuestions = 0;
//    public int score;

//    public GameObject QuizPanel;
//    public GameObject GoPanel;


//    private void Start()
//    {
//         totalQuestions = QnA.Count;
//         GoPanel.SetActive(false);
//         generateQuestion();

//    }

//    public void retry()
//    {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
//    }


//     public void GameOver()
//     {   
//         QuizPanel.SetActive(false);
//         GoPanel.SetActive(true);
//         ScoreTxt.text= score + "/" + totalQuestions;
//     }


//    public void correct()
//    {
//     if (currentQuestion >= 0 && currentQuestion < QnA.Count)
//         {
//             score += 1;
//             QnA.RemoveAt(currentQuestion);
//             StartCoroutine(WaitAndResetColors(1f));
//             // generateQuestion();
//         }
//         else
//         {
//             Debug.LogWarning("Attempted to remove an invalid question index.");
//         }
//    }

//    public void wrong()
//    {
//     //when you answer wrong
//     if (currentQuestion >= 0 && currentQuestion < QnA.Count)
//         {   
//             QnA.RemoveAt(currentQuestion);
//             StartCoroutine(WaitAndResetColors(1f));
//             // generateQuestion();
//         }
//         else
//         {
//             Debug.LogWarning("Attempted to remove an invalid question index.");
//         }
//    }

//    void SetAnswers()
//    {
//     for (int i = 0; i < options.Length; i++)
//     {
//         options[i].GetComponent<AnswerScript>().isCorrect = false;
//         options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = QnA[currentQuestion].Answers[i];
//         if(QnA[currentQuestion].CorrectAnswer == i+1)
//         {
//             options[i].GetComponent<AnswerScript>().isCorrect = true;
//         }
//     }
//    }

//     public void ResetButtonColors()
//     {
//         foreach (GameObject option in options)
//         {
//             Image buttonImage = option.GetComponent<Image>();
//             if (buttonImage != null)
//             {
//                 buttonImage.color = Color.black; 
//             }
//         }
//     }

//     IEnumerator WaitAndResetColors(float waitTime)
//     {
//     yield return new WaitForSeconds(waitTime); // Wait for the specified time
//     ResetButtonColors(); // Reset button colors after the wait
//     generateQuestion();
//     }

//    void generateQuestion()
//    {
//         if(QnA.Count > 0)
//         {
//             currentQuestion = Random.Range(0, QnA.Count);
//             QuestionTxt.text = QnA[currentQuestion].Question;
//             SetAnswers();
//         }
//         else{
//             Debug.Log("Out of Questions");
//             GameOver();
//         }
        
//    }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

public class QuizManager : MonoBehaviour
{
    public List<QuestionAndAnswer> QnA; // Original questions
    private List<QuestionAndAnswer> selectedQuestions = new List<QuestionAndAnswer>(); // Selected questions
    public GameObject[] options;
    public int currentQuestionIndex = 0; // Track the current question index

    public TMP_Text QuestionTxt;
    public TMP_Text ScoreTxt;

    public int score;
    public GameObject QuizPanel;
    public GameObject GoPanel;

    private string selectedPlanet; // Planet to filter questions

    private void Start()
    {
        GoPanel.SetActive(false);
        selectedPlanet = PlayerPrefs.GetString("SelectedPlanet", ""); // Retrieve the selected planet
        ShuffleQuestions(); // Shuffle and select questions for the chosen planet
        generateQuestion(); // Start generating questions
    }

    public void retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver()
    {   
        QuizPanel.SetActive(false);
        GoPanel.SetActive(true);
        ScoreTxt.text = score + "/" + selectedQuestions.Count; // Update to use selected questions count
    }

    public void correct()
    {
        score += 1;
        StartCoroutine(WaitAndResetColors(1f));
    }

    public void wrong()
    {
        StartCoroutine(WaitAndResetColors(1f));
    }

    void SetAnswers()
    {
        for (int i = 0; i < options.Length; i++)
        {
            options[i].GetComponent<AnswerScript>().isCorrect = false;
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = selectedQuestions[currentQuestionIndex].Answers[i];
            if (selectedQuestions[currentQuestionIndex].CorrectAnswer == i + 1)
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true;
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
        yield return new WaitForSeconds(waitTime);
        ResetButtonColors();
        NextQuestion(); // Proceed to the next question after waiting
    }

    void generateQuestion()
    {
        if (currentQuestionIndex < selectedQuestions.Count)
        {
            QuestionTxt.text = selectedQuestions[currentQuestionIndex].Question; // Display current question
            SetAnswers(); // Set the answer options
        }
        else
        {
            Debug.Log("Out of Questions");
            GameOver(); // End game if no more questions
        }
    }

    void ShuffleQuestions()
    {
        List<QuestionAndAnswer> filteredQuestions = QnA.FindAll(q => q.Planet == selectedPlanet); // Filter by planet

        if (filteredQuestions.Count == 0)
        {
            Debug.LogWarning("No questions available for the selected planet.");
            GameOver();
            return;
        }

        // Shuffle the filtered list
        List<QuestionAndAnswer> tempList = new List<QuestionAndAnswer>(filteredQuestions);
        int count = Mathf.Min(3, tempList.Count); // Get 3 questions or as many as are available

        for (int i = 0; i < tempList.Count; i++)
        {
            QuestionAndAnswer tmp = tempList[i];
            int r = Random.Range(i, tempList.Count);
            tempList[i] = tempList[r];
            tempList[r] = tmp;
        }

        // Select the first 3 questions
        selectedQuestions = tempList.GetRange(0, count);
        currentQuestionIndex = 0; // Reset to the first question
    }

    void NextQuestion()
    {
        currentQuestionIndex++; // Move to the next question
        generateQuestion(); // Generate the next question
    }
}