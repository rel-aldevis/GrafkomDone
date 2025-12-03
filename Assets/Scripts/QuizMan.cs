using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuizMan : MonoBehaviour
{
    public static int coinCount = 0;
    public int maxHits = 100;
    [SerializeField] private GameObject quizTriggerPrefab;

    [Header("UI & Prefab References")]
    public TextMeshProUGUI questionText; 
    public GameObject[] answerBlocks;
    public Transform[] spawnPoints;
    public Transform player;
    public Slider scoreSlider;
    private int currentScore = 0;
    public Slider energySlider;
    public int maxEnergy = 5;
    public int currentEnergy = 5;
    private int hitCount = 0;
    [SerializeField] GameObject coinDisplay;


    public Slider poinTarget;
    public TMP_Text coinText;
    public CollisionDetect collisionDetect;
    

    [Header("Game State")]
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip krisisEnergy;
    private AudioSource audioSource;
    public float quizEndDelay = 0.1f;
    private float nextTriggerZ = 40f;
    
    public bool IsQuizActive { get; private set; } = false;
    private int correctAnswerIndex;
    private List<GameObject> spawnedCubes = new List<GameObject>();
    private int[] currentAnswerValues = new int[3];
    //INI BISA DIHAPUS KLO BUY ENERGI ERROR
    public CoinEnergyManager coinEnergyManager;

 void Start()
{
    //INI BISA DIHAPUS KLO BUY ENERGI ERROR
    UpdateUI();
    if (questionText != null)
        questionText.gameObject.SetActive(false);

    if (answerBlocks.Length != 3 || spawnPoints.Length != 3)
    {
        Debug.LogError("Perlu 3 Prefab AnswerBlock dan 3 Spawn Points.");
    }

    if (scoreSlider != null)
        scoreSlider.value = 0;

    currentScore = 0;
    //coinCount = 0;

    currentEnergy = maxEnergy;

    if (energySlider != null)
    {
        energySlider.maxValue = maxEnergy;
        energySlider.value = currentEnergy;
    }

    if (poinTarget != null)
    {
        poinTarget.maxValue = 3;
        poinTarget.value = 0;
    }

    audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
}

    //INI BISA DIHAPUS KLO BUY ENERGI ERROR
    public void UpdateUI(){
        // if (coinText != null)
        //     coinText.text = "Koin: " + coinCount;
        if (energySlider != null)
            energySlider.value = currentEnergy;
            
    }
    public void AddCoins(int amount)
{
    coinCount += amount; // Pastikan variabel utama adalah coinCount/bukan currentCoins jika itu yang Anda gunakan di seluruh sistem
    Debug.Log("Coins collected: " + coinCount); // Tambahkan log untuk debugging jumlah koin
    UpdateUI(); // Selalu update UI setelah nilai berubah
}


     public void OnBuyEnergyClicked()
    {
        if (coinEnergyManager != null)
        {
            coinEnergyManager.OpenConfirmationPanel();
        }
        else
        {
            Debug.LogWarning("CoinEnergyManager belum di assign!");
        }
    }

    public void ReduceEnergy(int amount)
    {
        //if (hitCount >= maxHits) return;                // Stop jika sudah kena max hit

        hitCount++;
        currentEnergy -= amount;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);

        if (energySlider != null) 
        energySlider.value = currentEnergy;

        if ( currentEnergy <= 0) 
        {
            if(audioSource != null && krisisEnergy != null)
            audioSource.PlayOneShot(krisisEnergy);
            
            Debug.Log("Energi habis setelah " + hitCount + " kali tabrakan");
           // GameOver();
           //StartCoroutine(CollisionEnd()); //mulai dri awal
           //collisionDetect.StartCollisionEndCoroutine(); //tetap lanjut
           //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
           //collisionDetect.StopAllCoroutines();
          collisionDetect.GameOverSequence();
          
        }
    }

    public void StartQuiz(int num1, string op, int num2, int[] answerValues, int correctIdx)
    {
        if (IsQuizActive) return;
        IsQuizActive = true;
        currentAnswerValues = answerValues; 
        correctAnswerIndex = correctIdx;
        questionText.text = $"{num1} {op} {num2} = ?";
        questionText.gameObject.SetActive(true);
        SpawnAnswerBlocks();
    }

    private void SpawnAnswerBlocks()
    {
        float spawnZ = player.position.z + 25f;
        float spawnY = 0.5f;
        float[] xOffsets = new float[3] { -10, -6, -2 }; // kiri, tengah, kanan

        foreach (var cube in spawnedCubes) Destroy(cube);
        spawnedCubes.Clear();

        for (int i = 0; i < answerBlocks.Length; i++)
        {
            Vector3 spawnPos = new Vector3(xOffsets[i], spawnY, spawnZ);
            GameObject blockInstance = Instantiate(answerBlocks[i], spawnPos, answerBlocks[i].transform.rotation);
            spawnedCubes.Add(blockInstance);

            AnswerBlock answerBlock = blockInstance.GetComponent<AnswerBlock>();
            if (answerBlock != null)
            {
                answerBlock.SetAnswerValue(currentAnswerValues[i]);
                answerBlock.SetAnswerIndex(i);
            }
        }
    }
    
    public void AddScore(int value){
        currentScore += value;

        if(scoreSlider != null)
            scoreSlider.value = currentScore;

        if(poinTarget != null)
            poinTarget.value = currentScore;

        if(poinTarget != null && poinTarget.value >= poinTarget.maxValue){
            Debug.Log("Target poin tercapai! Pindah ke scene berikutnya.");
           // SceneManager.LoadScene(1);
           UnityEngine.SceneManagement.SceneManager.LoadScene(5);
        }
    }

    public void CheckAnswer(int selectedIndex)
    {
        if (!IsQuizActive) return; 
        Debug.Log($"Jawaban Dipilih: {selectedIndex}, Jawaban Benar: {correctAnswerIndex}");
        bool isCorrect = (selectedIndex == correctAnswerIndex);
        Debug.Log(isCorrect ? "Jawaban benar" : "Jawaban salah");
        if(isCorrect){
            AddScore(1);
            audioSource.PlayOneShot(correctSound);
        }
        else{
            audioSource.PlayOneShot(wrongSound);
        }
        StartCoroutine(DelayedEndQuiz(isCorrect));
    }

    private IEnumerator DelayedEndQuiz(bool isSuccess)
    {
        yield return new WaitForSeconds(quizEndDelay);
        IsQuizActive = false;
        EndQuizCleanup(isSuccess);

        if (isSuccess)
        {
            Debug.Log("Spawning next quiz trigger");
            SpawnNextTrigger();
        }else{
            SpawnNextTrigger();
        }
    }

    private void EndQuizCleanup(bool isSuccess)
    {
        foreach (GameObject cube in spawnedCubes)
        {
            Destroy(cube);
        }
        spawnedCubes.Clear();
        if (questionText != null) questionText.gameObject.SetActive(false);
    }

    private void SpawnNextTrigger()
    {
        Vector3 spawnPos = new Vector3(0, 0, Camera.main.transform.position.z + nextTriggerZ);
        Instantiate(quizTriggerPrefab, spawnPos, Quaternion.identity);
        Debug.Log("Spawned quiz trigger at z = " + spawnPos.z);
        //nextTriggerZ = 50f;
    }


    void Update()
    {
        coinDisplay.GetComponent<TMPro.TMP_Text>().text = "Coins: " + coinCount;
    }
    private IEnumerator CollisionEnd()
    {
        yield return new WaitForSeconds(krisisEnergy.length);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
