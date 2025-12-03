using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class QuizMan : MonoBehaviour
{
    [Header("UI & Prefab References")]
    // Anda bisa mengganti 'Text' dengan 'TextMeshProUGUI' jika menggunakan TextMeshPro
    public TextMeshProUGUI questionText; 
    public GameObject[] answerBlocks; // Array berisi 3 Prefab AnswerBlock (A, B, C)
    public Transform[] spawnPoints;   // 3 titik di jalan untuk spawn blok jawaban
    
    [Header("Game State")]
    public float blockSpawnDistance = 5f; // Jarak spawn blok dari trigger
    
    // Properti publik untuk status kuis
    public bool IsQuizActive { get; private set; } = false;

    // --- Data Kuis Saat Ini ---
    private int correctAnswerIndex; // Indeks (0, 1, atau 2) dari blok yang benar
    private List<GameObject> spawnedCubes = new List<GameObject>(); // Untuk melacak kubus yang aktif
    
    void Start()
    {
        // Sembunyikan UI saat game dimulai
        if (questionText != null)
        {
            questionText.gameObject.SetActive(false);
        }

        // Validasi setup
        if (answerBlocks.Length != 3 || spawnPoints.Length != 3)
        {
            Debug.LogError("QuizManager membutuhkan tepat 3 Prefab AnswerBlock dan 3 Transform Spawn Points.");
        }
    }

    // Dipanggil oleh QuizTrigger
    public void StartQuiz(int num1, string op, int num2, int correctIdx)
    {
        if (IsQuizActive) return;

        IsQuizActive = true;
        
        // 1. Tampilkan Soal
        questionText.text = $"{num1} {op} {num2} = ?";
        questionText.gameObject.SetActive(true);
        correctAnswerIndex = correctIdx;

        Debug.Log($"Kuis Baru: {questionText.text} Jawaban Benar di Index: {correctAnswerIndex}");

        // 2. Spawn Blok Jawaban
        SpawnAnswerBlocks();
        
        // Opsional: Pause pergerakan pemain di sini
        // Example: FindObjectOfType<PlayerMovement>().isPaused = true;
    }

    private void SpawnAnswerBlocks()
    {
        // Hancurkan kubus lama (jika ada) - Seharusnya tidak ada
        foreach (var cube in spawnedCubes)
        {
            Destroy(cube);
        }
        spawnedCubes.Clear();

        // Spawn 3 blok, satu per satu
        for (int i = 0; i < answerBlocks.Length; i++)
        {
            GameObject blockPrefab = answerBlocks[i];
            
            // Spawn di posisi Z yang sedikit di depan spawnPoints (agar terlihat jelas)
            Vector3 spawnPos = spawnPoints[i].position + new Vector3(0, 0, blockSpawnDistance);
            
            GameObject block = Instantiate(blockPrefab, spawnPos, blockPrefab.transform.rotation);
            spawnedCubes.Add(block);
        }
    }

    // Dipanggil oleh AnswerBlock saat pemain menabrak/mengklik
    public void CheckAnswer(int selectedIndex)
    {
        if (!IsQuizActive) return;

        IsQuizActive = false; // Nonaktifkan kuis segera setelah jawaban dipilih
        
        if (selectedIndex == correctAnswerIndex)
        {
            Debug.Log("BENAR! Lanjut.");
            // Logika Lanjut Game: (Misalnya, tambahkan skor, reset speed)
            EndQuiz(true);
        }
        else
        {
            Debug.Log("SALAH! Game Over.");
            // Logika Game Over: (Misalnya, tampilkan layar Game Over)
            EndQuiz(false);
        }
    }
    
    private void EndQuiz(bool isSuccess)
    {
        // 1. Hancurkan semua kubus yang tersisa
        foreach (GameObject cube in spawnedCubes)
        {
            // Tambahkan visual feedback (misalnya, ganti warna sebelum dihancurkan)
            Destroy(cube, 0.1f); // Hancurkan setelah sedikit penundaan visual
        }
        spawnedCubes.Clear();

        // 2. Sembunyikan UI
        questionText.gameObject.SetActive(false);

        // 3. Reset atau Atur Ulang Pemain/Game
        // Contoh: if (!isSuccess) { GetComponent<GameController>().GameOver(); }
        // Example: FindObjectOfType<PlayerMovement>().isPaused = false;
        
        if (!isSuccess)
        {
            // Implementasikan fungsi Game Over sesuai game Anda
            Debug.Log("FUNGSI GAME OVER DIPANGGIL!");
            // SceneManager.LoadScene("GameOverScene");
        }
        
        // Hancurkan trigger yang memicu kuis ini (Opsional, tergantung desain level)
        QuizTrigger activeTrigger = FindObjectOfType<QuizTrigger>();
        if (activeTrigger != null)
        {
            Destroy(activeTrigger.gameObject);
        }
    }
}