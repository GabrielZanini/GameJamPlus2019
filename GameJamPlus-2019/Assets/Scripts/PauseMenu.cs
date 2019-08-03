using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;

    //referencia a ui
    public GameObject pauseMenuUi;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        //disable game object
        pauseMenuUi.SetActive(false);
        //back time
        Time.timeScale = 1f;
        //despausa
        GameIsPaused = false;
    }

    void Pause()
    {
        //enable game object
        pauseMenuUi.SetActive(true);
        //freeze time
        Time.timeScale = 0f;
        //pausa o jogo
        GameIsPaused = true;

   
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("loading");
    }
}
