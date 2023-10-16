using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSceneController : MonoBehaviour
{    
    public static Vector3 SceneDimensions;

    public static bool StartLevel;

    public Canvas endGameUI;

    private bool setVariables;

    //keeps the highest combo count
    public static int highestComboCount;

    public static float globalEnemySpeed;

    public static float globalPlayerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartLevel = false;

        SceneDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        highestComboCount = 0;

        Debug.Log(SceneDimensions);
    }

    private void Update()
    {
        if(!setVariables)
        {
            globalPlayerSpeed = Player.PlayerSpeed;
            Debug.Log("Global Player Speed - " + globalPlayerSpeed);
            Debug.Log("Global Enemy Speed - " + globalEnemySpeed);

        }
    }

    //Displays the end UI when the game is over
    public void endGame()
    {
        Debug.Log("End Game Triggered");
        Instantiate(endGameUI);
    }

    
}
