using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionDetect : MonoBehaviour
{
    [Header("Quiz System")]
    [SerializeField] QuizMan quizMan;

    [Header("Feedback & Player Stuff")]
    [SerializeField] GameObject thePlayer;
    [SerializeField] GameObject playerAnim;
    [SerializeField] AudioSource collisionFX;
    [SerializeField] GameObject maincam;
    [SerializeField] GameObject fadeout;
    [SerializeField] private AudioClip oughSound;

    private AudioSource audioSource;

    [Header("Scene Management")]
    [SerializeField] int sceneToLoad;   // ⬅️ bisa diisi dari inspector

    private bool isColliding = false;
    private bool isGameOver = false;

    void Awake()
    {
        if (quizMan == null)
            quizMan = FindObjectOfType<QuizMan>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (isGameOver) return;

        if (!isColliding)
        {
            isColliding = true;

            if (audioSource != null && oughSound != null)
                audioSource.PlayOneShot(oughSound);

            quizMan.ReduceEnergy(1);
        }
    }

    // Dipanggil dari QuizMan setelah energy = 0
    public void GameOverSequence()
    {
        if (isGameOver) return;
        isGameOver = true;

        StopAllCoroutines();
        StartCoroutine(CollisionEnd());
    }

    IEnumerator CollisionEnd()
    {
        collisionFX.Play();
        thePlayer.GetComponent<PlayerMovement>().enabled = false;
        playerAnim.GetComponent<Animator>().Play("Stumble Backwards");
        maincam.GetComponent<Animator>().Play("CollisionCam");

        yield return new WaitForSeconds(3);

        fadeout.SetActive(true);
        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(sceneToLoad); // ⬅️ load scene dari Inspector
    }
}
