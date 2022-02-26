using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetButtonDown("Pause"))
        {
            Time.timeScale = 0;
            //UI show up

        }    
    }
    void Resume()
    {
        Time.timeScale = 1;
        //UI disappear
    }
    void ReturnToMainMenu()
    {
        Time.timeScale = 1;
        //Load MainMenu Scene
    }

    void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Music()
    {

    }
}
