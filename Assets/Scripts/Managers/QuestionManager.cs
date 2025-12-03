using System.Collections.Generic;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public static QuestionManager instance;
    public List<QuestionData> allQuestions = new List<QuestionData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            LoadQuestions(); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadQuestions()
    {
        // Tambahkan 5 pertanyaan di sini
        allQuestions.Add(new QuestionData
        {
            questionText = "Siapakah pendiri perusahaan Apple?",
            correctAnswer = "Steve Jobs",
            incorrectAnswers = new List<string> { "Bill Gates"}
        });

        // ... (lanjutkan 4 pertanyaan lainnya di sini) ...
    }

    public QuestionData GetRandomQuestion()
    {
        if (allQuestions.Count == 0) return null;
        int randomIndex = Random.Range(0, allQuestions.Count);
        return allQuestions[randomIndex];
    }
}