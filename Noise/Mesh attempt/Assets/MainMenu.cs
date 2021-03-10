using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){  //For play button
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame(){ //For Quit button

        Debug.Log("Quit pressed. Doesnt work in editor.");
        Application.Quit();
        
    }
}
