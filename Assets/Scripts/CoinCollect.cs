using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [SerializeField] AudioSource DropCoin;
    // public MasterInfo masterInfo;
    public QuizMan quizMan;
    private void OnTriggerEnter(Collider other)
    {
        DropCoin.Play();
        QuizMan.coinCount += 1;
        this.gameObject.SetActive(false);
    }
}