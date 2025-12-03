using UnityEngine;
using TMPro;
using System.Collections;

public class AnswerBlock : MonoBehaviour
{
    [Header("Referensi UI & Manager")]
    public TextMeshPro answerText;
    private QuizMan quizManager;

    [Header("Data Jawaban")]
    public int answerValue;
    private int answerIndex;

    [Header("Efek Tabrakan")]
    [SerializeField] GameObject thePlayer;
    [SerializeField] GameObject playerAnim;
    [SerializeField] AudioSource collisionFX;
    [SerializeField] GameObject maincam;
    [SerializeField] GameObject fadeout;

    void Start()
    {
        quizManager = FindFirstObjectByType<QuizMan>();
        if (quizManager == null)
        {
            Debug.LogError("QuizMan tidak ditemukan.");
        }
    }

    public void SetAnswerValue(int value)
    {
        answerValue = value;
        if (answerText != null)
        {
            answerText.text = value.ToString();
        }
    }

    public void SetAnswerIndex(int index)
    {
        answerIndex = index;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Blok " + answerIndex + " di-trigger oleh Player.");

            if (quizManager != null)
            {
                quizManager.CheckAnswer(answerIndex);

                // Efek collision hanya jika jawaban salah
                
                {
                    StartCoroutine(CollisionFeedback());
                }
            }
        }
    }

    IEnumerator CollisionFeedback()
    {
        if (collisionFX != null) collisionFX.Play();

        //if (thePlayer != null)
        //{
        //    PlayerMovement pm = thePlayer.GetComponent<PlayerMovement>();
        //    if (pm != null) pm.DisableMovement(); // aman, pakai flag
        //}
        if (thePlayer != null) thePlayer.GetComponent<PlayerMovement>().enabled = false;

        if (playerAnim != null) playerAnim.GetComponent<Animator>().Play("Stumble Backwards");
        if (maincam != null) maincam.GetComponent<Animator>().Play("CollisionCam");

        yield return new WaitForSeconds(3);

        if (fadeout != null) fadeout.SetActive(true);

        //yield return new WaitForSeconds(2);

        //if (quizManager != null) quizManager.GameOver();
    }
}