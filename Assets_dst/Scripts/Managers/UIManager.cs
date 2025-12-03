using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq; // Penting untuk fungsi OrderBy (mengacak list)
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI References")]
    //public GameObject questionPanel; 
    public Image ImageQuestion;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons; 

    private string correctAns;

    private void Awake()
    {
        // === Singleton Initialization ===
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Pastikan panel tersembunyi
        if (ImageQuestion != null) 
            ImageQuestion.gameObject.SetActive(false); 
    }

    // FUNGSI INI DIIMPLEMENTASIKAN
    public void ShowQuestionPanel(QuestionData data)
    {
        if (data == null) return;
        
        // 1. Aktifkan Panel UI dan Tampilkan Pertanyaan
        ImageQuestion.gameObject.SetActive(true); 
        questionText.text = data.questionText;
        correctAns = data.correctAnswer;
        
        // TODO: Nonaktifkan pergerakan Player di sini

        // 2. Menggabungkan dan Mengacak Jawaban
        List<string> answers = new List<string>();
        answers.Add(data.correctAnswer);
        answers.AddRange(data.incorrectAnswers);
        
        // Acak urutan jawaban (Menggunakan LINQ)
        answers = answers.OrderBy(x => Random.value).ToList();

        // 3. Menetapkan Teks dan Listener Tombol
        for (int i = 0; i < answerButtons.Length; i++)
        {
            if (i < answers.Count)
            {
                // Ambil komponen TextMeshProUGUI dari tombol
                TextMeshProUGUI buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                
                if (buttonText != null)
                {
                    buttonText.text = answers[i];
                }

                // Reset dan Tambahkan Listener (Pastikan event OnClick dikaitkan dengan CheckAnswer)
                answerButtons[i].onClick.RemoveAllListeners();
                
                string currentAnswer = answers[i]; 
                
                answerButtons[i].onClick.AddListener(() => CheckAnswer(currentAnswer));
                
                answerButtons[i].gameObject.SetActive(true); 
            }
            else
            {
                // Sembunyikan tombol jika tidak ada cukup jawaban
                answerButtons[i].gameObject.SetActive(false);
            }
        }
    }

    // FUNGSI INI DIIMPLEMENTASIKAN
    public void CheckAnswer(string playerAnswer)
    {
        // 1. Sembunyikan panel
        ImageQuestion.gameObject.SetActive(false); 

        // 2. Logika Pengecekan
        if (playerAnswer == correctAns)
        {
            Debug.Log($"Jawaban BENAR! Anda memilih: {playerAnswer}");
            // TODO: Logika Reward
        }
        else
        {
            Debug.LogWarning($"Jawaban SALAH! Jawaban benar adalah: {correctAns}");
            // TODO: Logika Penalty
        }
        
        // TODO: Aktifkan kembali pergerakan Player di sini
    }
}