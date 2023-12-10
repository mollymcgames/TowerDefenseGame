using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameOverScript : MonoBehaviour
{

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1 ); //might need to change this logic as it loads the next screen in the build index
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
