using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageControl : MonoBehaviour
{

    private void Start()
    {

    }

    void Update()
    {

    }
    public void Stage1()
    {
        SceneManager.LoadScene(2);
    }
    public void Stage2()
    {
        SceneManager.LoadScene(3);
    }
    public void Home()
    {
        SceneManager.LoadScene(0);
    }
    public void StageSelect()
    {
        SceneManager.LoadScene(1);
    }
    public void GameOver()
    {
        Debug.Log("Game Over!");
        SceneManager.LoadScene(4);
    }
}