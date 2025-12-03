using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinEnergyManager : MonoBehaviour
{
    private int energyCost = 40;
    public int energyAmount = 0;

    [SerializeField] private TMP_Text confirmationText;
    [SerializeField] private Button buyEnergyButton;

    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;

    public QuizMan quizMan;

    void Start()
    {
        // quizMan = FindObjectOfType<QuizMan>();
        quizMan = FindAnyObjectByType<QuizMan>();

        buyEnergyButton.onClick.AddListener(OpenConfirmationPanel);
        confirmButton.onClick.AddListener(ConfirmPurchase);
        cancelButton.onClick.AddListener(CloseConfirmationPanel);

        CloseConfirmationPanel();
    }

    public void OpenConfirmationPanel()
    {
        Time.timeScale = 0f;
        confirmationPanel.SetActive(true);
        if (confirmationText != null)
            confirmationText.text = "Apakah ingin membeli energi dengan 40 koin?";
        buyEnergyButton.interactable = false;
    }

    private void CloseConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
        buyEnergyButton.interactable = true;
        Time.timeScale = 1f;
    }

    private void ConfirmPurchase()
    {
        if (quizMan == null)
        {
            Debug.LogError("QuizMan tidak ada");
            CloseConfirmationPanel();
            return;
        }
        if (QuizMan.coinCount >= energyCost && quizMan.currentEnergy < quizMan.maxEnergy)
        {
            QuizMan.coinCount -= energyCost;
            quizMan.currentEnergy = quizMan.maxEnergy;
            quizMan.UpdateUI();
            Debug.Log("Pembelian energi berhasil");
            Debug.Log("Sisa koin" + QuizMan.coinCount);
        }
        else if (quizMan.currentEnergy >= quizMan.maxEnergy){
            Debug.Log("Energi sudah penuh");  
        }
        else{
            Debug.Log("Koin tidak cukup");
        }
        CloseConfirmationPanel();
    }
 }


