using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool gamePaused = false;
    GameObject pausePanel;
    // Start is called before the first frame update
    void Start()
    {
        //check if PauseManager gameObject is found or not
        pausePanel = GameObject.Find("PauseManager");
        if (pausePanel != null)
        {
            pausePanel = gameObject.transform.Find("PausePanel").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
    }

    public void StartGame()
    {


        SceneManager.LoadScene("PartyLevelScene");
        

    }

    public void MainMenu()
    {

        SceneManager.LoadScene("MainMenuScene");
        Time.timeScale = 1;


    }

    public void PauseGame()
    {

        if (Input.GetButtonDown("Pause"))
        {
            gamePaused = !gamePaused;
            if (gamePaused)
            {
                pausePanel.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                pausePanel.SetActive(false);
                Time.timeScale = 1;
            }

        }
        
    }

    public void UnpauseGame()
    {

        gamePaused = false;
        pausePanel.SetActive(false);
        Time.timeScale = 1;

    }

    public void RestartGame()
    {
        UnpauseGame();

        if (SceneManager.GetActiveScene().name != null && SceneManager.GetActiveScene().name != "")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
        {
            SceneManager.LoadScene("MainMenuScene");
        }
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
