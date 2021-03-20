using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPause = false;

    public GameObject PauseMenuUI;



    void Update() {
        if ( Input.GetKeyDown(KeyCode.Escape)){
            if ( GameIsPause){
                Resume();
            }else
            {
                Pause();
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
    }

    public void Resume(){
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPause = false;

    }

    void Pause(){
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPause = true;
    }

    public void LoadMenu(){
        Debug.Log("Loading Menu..");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        GameIsPause = false;
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame(){
        Debug.Log("Quitting Game..");
        Application.Quit();
    }
}

