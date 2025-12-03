using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuizTrigger : MonoBehaviour
{
    [Header("Soal Kuis")]
    public int number1;
    public string operation = "+";
    public int number2;

    [Header("Jawaban Blok (A, B, C)")]
    public int[] optionValues = new int[3];

    private QuizMan quizManager;

    void Start()
    {
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning("Collider pada QuizTrigger harus disetel sebagai 'Is Trigger'.");
            col.isTrigger = true;
        }

        quizManager = FindFirstObjectByType<QuizMan>();
        if (quizManager == null)
        {
            Debug.LogError("QuizMan tidak ditemukan.");
        }
    }

    private void GenerateRandomQuiz()
    {
        // 1. Generate Soal
        number1 = Random.Range(5, 15);
        number2 = Random.Range(2, 10);
        //operation = (Random.value < 0.5f) ? "+" : "-" : "x"; 
        //string[] operators = { "+", "-", "x",":" };
        string[] operators = { "+", "-", "x" };
        operation = operators[Random.Range(0, operators.Length)];

        if (operation == "-" && number1 < number2)
        {
            int temp = number1;
            number1 = number2;
            number2 = temp;
        }

        // 2. Hitung Jawaban Benar
        int correctResult = CalculateResult(number1, operation, number2);

        // 3. Generate Opsi Jawaban (Termasuk yang salah)
        List<int> options = new List<int>();
        options.Add(correctResult);

        while (options.Count < 3)
        {
            int offset = Random.Range(-3, 4);
            if (offset == 0) continue;

            int wrongAnswer = correctResult + offset;

            if (wrongAnswer >= 0 && !options.Contains(wrongAnswer))
            {
                options.Add(wrongAnswer);
            }
        }

        options = options.OrderBy(x => Random.value).ToList();

        for (int i = 0; i < options.Count; i++)
        {
            optionValues[i] = options[i];
        }

        // DEBUG CONSOLE: Tampilkan soal random yang dibuat
        Debug.Log($"Soal Random Dibuat: {number1} {operation} {number2} = ? Opsi: {optionValues[0]}, {optionValues[1]}, {optionValues[2]} (Benar: {correctResult})");
    }

    private int CalculateResult(int n1, string op, int n2)
    {
        if (op == "+") return n1 + n2;
        if (op == "-") return n1 - n2;
        if (op == "x") return n1 * n2;
        //if (op == ":") return n1 / n2;

        return 0;
    }

    public void GenerateAndStartQuiz()
    {
        if (quizManager == null || quizManager.IsQuizActive) return;

        // 1. Generate soal baru
        GenerateRandomQuiz();

        // 2. Cari index jawaban benar
        int correctResult = CalculateResult(number1, operation, number2);
        int correctIndex = -1;
        for (int i = 0; i < optionValues.Length; i++)
        {
            if (optionValues[i] == correctResult)
            {
                correctIndex = i;
                break;
            }
        }

        if (correctIndex != -1)
        {
            quizManager.StartQuiz(number1, operation, number2, optionValues, correctIndex);
        }
        else
        {
            Debug.LogError("Kesalahan: Jawaban benar tidak ditemukan di dalam optionValues array setelah generate.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && quizManager != null && !quizManager.IsQuizActive)
        {
            GenerateAndStartQuiz();
        }
    }
}