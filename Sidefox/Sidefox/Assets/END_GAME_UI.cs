using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;



public class END_GAME_UI : MonoBehaviour
{
    public TMP_Text comboLevel;

    public Text totalEnemiesKilled;

    public Text playerHealth;

        // Start is called before the first frame update
        void Start()
    {
        comboLevel.text = comboLevel.text + " - " + TestSceneController.highestComboCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void Restart()
    {
        TestSceneController.StartLevel = false;
        CoPilotText.sentenceIndex = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
