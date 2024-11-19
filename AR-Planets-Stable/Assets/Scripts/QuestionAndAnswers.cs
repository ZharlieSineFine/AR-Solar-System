// [System.Serializable]
// public class QuestionAndAnswer
// {
//     public string Question;
//     public string[] Answers;
//     public int CorrectAnswer;
// }

[System.Serializable]
public class QuestionAndAnswer
{
    public string Question;
    public string[] Answers;
    public int CorrectAnswer; // Assuming this is 1-based index
    public string Planet; // New field to identify the planet
}