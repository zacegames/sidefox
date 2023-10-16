using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Used to re-load the scene
using UnityEngine.SceneManagement;

public class SideScrollingSceneController : MonoBehaviour
{

    //For placement of the enemy player
    public GameObject Enemy;

    //Used to calculate how often the enemies will spawn in the world
    float EnemySpawnTimer;

    //Used store the screen dimensions to be used to see if object is on screen
    public static Vector3 ScreenDimensions;

    //Used to choose which type of enemy to spawn
    public int EnemySpawnType;

    //Used to have pattern movement 4 target this position to not have the enemy keep juttering around
    public static Vector3 pat4playertargetpos;


    // Start is called before the first frame update
    void Start()
    {
     //   EnemySpawnTimer = 0;

        //Set the screen dimensions in world units
        ScreenDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

        //Debug.Log("Sceen World Sizes = " + ScreenDimensions);
    }

    // Update is called once per frame
    void Update()
    {
        //Count down to spawn the enemy
        EnemySpawnTimer -= Time.deltaTime;
/*
        if(EnemySpawnTimer <= 0)
        {
            //Debug.Log("Spawn Enemy");

            if (EnemySpawnType == 0)
            {
                float randomHeight = Random.Range(0.15f, 0.85f);
                Instantiate(Enemy, Camera.main.ViewportToWorldPoint(new Vector3(1.1f, randomHeight, 10f)), Quaternion.identity);
                Enemy.GetComponent<Enemy_1>().EnemyMovementPattern = 0;
            }

            if(EnemySpawnType == 1)
            {
                //choose whether to spawn the enemy at the top or bottom of the screen
                float YStartPoint;

                int TopOrBottom = Random.Range(1,3);

                //Debug.Log("TOB = " + TopOrBottom);

                if (TopOrBottom == 1)
                {
                    YStartPoint = -ScreenDimensions.y + (Enemy.gameObject.GetComponent<Renderer>().bounds.size.y / 2);
                    Enemy.GetComponent<Enemy_1>().EnemyMovementPattern = 1;
                    Instantiate(Enemy, new Vector3(ScreenDimensions.x + 1f, YStartPoint, 0), Quaternion.identity);
                }
                else
                {
                    YStartPoint = ScreenDimensions.y - (Enemy.gameObject.GetComponent<Renderer>().bounds.size.y / 2);
                    Enemy.GetComponent<Enemy_1>().EnemyMovementPattern = 2;
                    Instantiate(Enemy, new Vector3(ScreenDimensions.x + 1f, YStartPoint, 0), Quaternion.identity);
                }
            }

            if (EnemySpawnType == 2)
            {
                int StartPoint = Random.Range(8, 12);
                Instantiate(Enemy, new Vector3((float)StartPoint, 0f, 0f), Quaternion.identity);
                Enemy.GetComponent<Enemy_1>().EnemyMovementPattern = 3;
            }

            if (EnemySpawnType == 3)
            {
                int StartPoint = 6;//Random.Range(8, 12);
                //Set the start and end positions for the enemy to move along. They need to be static for correct movement
                Enemy_1.pat4startpos = new Vector3((float)StartPoint, 0f, 0f);
                Enemy_1.pat4endpos = new Vector3 (Player.PlayerPosition.x +2f, Player.PlayerPosition.y, Player.PlayerPosition.z);
                Instantiate(Enemy, Enemy_1.pat4startpos, Quaternion.identity);
                pat4playertargetpos = Player.PlayerPosition;
                Enemy.GetComponent<Enemy_1>().EnemyMovementPattern = 4;
            }

            //Reset the spawn timer
            EnemySpawnTimer = 5f;
        }
        */

        //Restart the scene if the middle pad (PS4) or ] key pressed (keyboard)
        if (Input.GetKey("joystick button 13") || Input.GetKey(KeyCode.RightBracket))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
