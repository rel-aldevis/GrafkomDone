using UnityEngine;

public class QuestionTrigger : MonoBehaviour
{
    // Fungsi ini dipanggil ketika Collider lain masuk area Trigger
    void OnTriggerEnter(Collider other)
    {
        // PENTING: Cek apakah log ini muncul di Console Unity
        Debug.Log("Trigger Terpanggil oleh Objek Bernama: " + other.gameObject.name + 
              " dengan Tag: " + other.tag, other.gameObject);

        // Pastikan Player punya tag "Player"
        if (other.CompareTag("Player")) 
        {
            // Panggil logika pertanyaan dan nonaktifkan trigger
            if (QuestionManager.instance != null && UIManager.instance != null)
            {
                QuestionData qData = QuestionManager.instance.GetRandomQuestion();
                if (qData != null)
                {
                    UIManager.instance.ShowQuestionPanel(qData);
                }
            }
            
            // Menonaktifkan Collider agar tidak terpicu lagi
            GetComponent<Collider>().enabled = false; 
        }
    }
}