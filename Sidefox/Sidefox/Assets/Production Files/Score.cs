using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    /* SCORE WORKS AS FOLLOWS:
     * - Total enemies destroyed by how many are in the level
     * - Total hits taken by the player
     * - Total time taken to kill the boss 
     */

    private float TotalEnemiesInLevel;

    public static float TotalEnemiesKilled;

    private float pMaxHealth;

    public static int ScoreCount;

    private Text ScoreText;
    // Start is called before the first frame update
    void Start()
    {
        ScoreText = GetComponent<Text>();

        TotalEnemiesInLevel = GameObject.FindGameObjectsWithTag("Enemy").Length;
        TotalEnemiesKilled = GameObject.FindGameObjectsWithTag("Enemy").Length;
        pMaxHealth = Player.Overall_pHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Overall Health = " + Player.Overall_pHealth);

        //Debug.Log("Health = " + (Player.Overall_pHealth/Player.pHealth));

        //Debug.Log((TotalEnemiesInLevel / TotalEnemiesKilled) + (Player.Overall_pHealth / Player.pHealth));

        float CalulateScore = (TotalEnemiesInLevel / TotalEnemiesKilled) + (Player.Overall_pHealth / Player.pHealth);

        
        if (CalulateScore == 2)
        {
            ScoreText.text = "Score - S";
        }
        else
        {
            ScoreText.text = "Score - A++";
        }
    }
}
