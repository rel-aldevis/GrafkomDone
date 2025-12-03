//using TMPro;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class GameOverUI : MonoBehaviour
//{
//    [SerializeField] private TMP_Text coinDisplay;
//    [SerializeField] private int coinsNeededToContinue = 30;
//    [SerializeField] private TMP_Text notEnoughText; // optional feedback

//    void Start()
//    {
//        int current = PlayerPrefs.GetInt("Coins", 0);
//        if (coinDisplay != null) coinDisplay.text = $"Coins: {current}";
//        if (notEnoughText != null) notEnoughText.gameObject.SetActive(false);
//    }

//    public void OnYesButtonClicked()
//    {
//        // gunakan API MasterInfo.SpendCoins sehingga semua logic disentralisasi
//        if (MasterInfo.SpendCoins(coinsNeededToContinue))
//        {
//            // sukses bayar — load kembali ke gameplay scene yang terakhir
//            int lastScene = PlayerPrefs.GetInt("LastGameSceneIndex", 1);
//            Debug.Log($"[GameOverUI] Continue: coins after pay = {PlayerPrefs.GetInt("Coins", -999)}, loading scene {lastScene}");
//            SceneManager.LoadScene(lastScene);
//        }
//        else
//        {
//            Debug.Log("[GameOverUI] Koin tidak cukup untuk melanjutkan!");
//            if (notEnoughText != null) notEnoughText.gameObject.SetActive(true);
//        }
//    }

//    public void OnNoButtonClicked()
//    {
//        // restart atau balik ke menu
//        SceneManager.LoadScene(0);
//    }
//}
