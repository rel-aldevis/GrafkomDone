using UnityEngine;
using TMPro;

public class AnswerBlock : MonoBehaviour
{
    // Indeks jawaban untuk membedakan blok: 0 (A), 1 (B), 2 (C)
    [Header("Configuration")]
    public int answerIndex = 0;

    // PENTING: Pastikan komponen TextMeshProUGUI dari objek anak (Child) ditarik ke slot ini di Inspector!
    [Header("References")]
    public TextMeshProUGUI answerText; 

    private QuizMan quizManager; 

    void Start()
    {
        // Cari QuizMan
        quizManager = FindFirstObjectByType<QuizMan>(); 
        if (quizManager == null)
        {
            Debug.LogError("QuizMan tidak ditemukan di scene. Pastikan objek QuizMan sudah ada.");
        }
        
        // Memastikan teks direset dan referensi ada
        if (answerText != null){
            answerText.text = "Mengisi Jawaban..."; // Teks sementara untuk debugging
        } else {
            // Ini adalah peringatan kunci jika teks tidak muncul
            Debug.LogError($"[SETUP ERROR] AnswerBlock pada objek '{gameObject.name}' memiliki referensi answerText yang kosong. Pastikan TextMeshProUGUI ditarik ke slot di Inspector!");
        }
    }

    // Metode ini dipanggil oleh QuizMan untuk mengisi angka di kubus
    public void SetAnswerValue(int value)
    {
        if (answerText != null)
        {
            answerText.text = value.ToString();
        }
    }

    // Metode untuk mendeteksi tabrakan
    private void OnTriggerEnter(Collider other)
    {
        // Pastikan pemain memiliki tag "Player"
        if (other.CompareTag("Player"))
        {
           if (quizManager != null)
           {
                // Beri tahu Quiz Manager bahwa blok ini ditabrak
                quizManager.CheckAnswer(answerIndex);
           }
           // Hancurkan blok setelah ditabrak 
           Destroy(gameObject); 
        }
    }
}