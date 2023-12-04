using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapScript : MonoBehaviour
{

    public Button Level1Button;
    public Button Level2Button;
    public Button Level3Button;
    public Button Level4Button;

    void Start()
    {
        Level2Button.interactable = IsLevelCompleted("Test");
        Level3Button.interactable = IsLevelCompleted("LevelTwo");
        Level4Button.interactable = IsLevelCompleted("LevelThree");
    }

    private bool IsLevelCompleted(string levelName)
    {
        return PlayerPrefs.GetInt(levelName + "_Completed", 0) == 1; //Completed or finished
    }
}
