using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuizTrigger : MonoBehaviour
{
    [Header("Soal Kuis")]
    public int number1;
    public string operation = "+"; // Tanda operasi: "+" atau "-"
    public int number2;
    
    [Header("Jawaban Blok (A, B, C)")]
    // Nilai-nilai ini akan dipetakan ke 3 blok jawaban (misalnya: 10, 8, 12)
    public int[] optionValues = new int[3]; 

    private QuizMan quizManager; // Diubah dari QuizManager ke QuizMan
    private bool hasBeenTriggered = false;
    
    void Start()
    {
        // Pastikan Collider adalah Trigger
        Collider col = GetComponent<Collider>();
        if (col != null && !col.isTrigger)
        {
            Debug.LogWarning("Collider pada QuizTrigger harus disetel sebagai 'Is Trigger'.");
            col.isTrigger = true;
        }

        quizManager = FindObjectOfType<QuizMan>(); // Diubah dari QuizManager ke QuizMan
        if (quizManager == null)
        {
            Debug.LogError("QuizMan tidak ditemukan.");
        }
        
        // Generate dan atur soal dan jawaban saat start (jika belum diisi manual di Inspector)
        GenerateRandomQuiz();
    }
    
    // Fungsi untuk menghasilkan soal dan jawaban yang unik
    private void GenerateRandomQuiz()
    {
        // 1. Generate Soal (Angka 2-10)
        number1 = Random.Range(5, 15);
        number2 = Random.Range(2, 10);
        operation = (Random.value < 0.5f) ? "+" : "-"; 

        // Pastikan hasil pengurangan tidak negatif
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

        // Generate 2 jawaban palsu yang dekat tapi berbeda
        while (options.Count < 3)
        {
            // Selisih antara -2 hingga 3 (tidak termasuk 0)
            int offset = Random.Range(-3, 4);
            if (offset == 0) continue; 
            
            int wrongAnswer = correctResult + offset;
            
            // Pastikan tidak negatif dan unik
            if (wrongAnswer >= 0 && !options.Contains(wrongAnswer))
            {
                options.Add(wrongAnswer);
            }
        }

        // Acak urutan opsi
        options = options.OrderBy(x => Random.value).ToList();
        
        // Isi array optionValues
        for(int i = 0; i < options.Count; i++)
        {
            optionValues[i] = options[i];
        }
        
        Debug.Log($"Soal Dibuat: {number1} {operation} {number2}. Opsi: {options[0]}, {options[1]}, {options[2]}");
    }

    private int CalculateResult(int n1, string op, int n2)
    {
        if (op == "+") return n1 + n2;
        if (op == "-") return n1 - n2;
        return 0;
    }

    // Mendeteksi pemain masuk ke area trigger
    private void OnTriggerEnter(Collider other)
    {
        // Pastikan pemain memiliki tag "Player" dan trigger belum terpicu
        if (other.CompareTag("Player") && !hasBeenTriggered)
        {
            if (quizManager == null) return;
            
            hasBeenTriggered = true;

            // Cari index mana di array optionValues yang mengandung jawaban benar
            int correctResult = CalculateResult(number1, operation, number2);
            int correctIndex = -1;
            for (int i = 0; i < optionValues.Length; i++)
            {
                // Jika nilai opsi sesuai dengan hasil yang benar, ini adalah indeks blok yang benar
                if (optionValues[i] == correctResult)
                {
                    correctIndex = i;
                    break;
                }
            }

            if (correctIndex != -1)
            {
                // Panggil QuizManager untuk memulai kuis
                quizManager.StartQuiz(number1, operation, number2, correctIndex);
                
                // Opsional: Hancurkan trigger ini setelah terpicu
                // Destroy(gameObject, 0.1f);
            }
            else
            {
                Debug.LogError("Kesalahan: Jawaban benar tidak ditemukan di dalam optionValues array.");
            }
        }
    }
}