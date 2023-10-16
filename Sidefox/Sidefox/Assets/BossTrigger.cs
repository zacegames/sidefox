using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTrigger : MonoBehaviour
{
    //Instantiates the boss
    public GameObject Boss;

    //Flashes the warning Logo
    public Image Warning;

    //Used to see if the flashing warning is going up or down
    private bool WarningAlphaUp;

    //Used to store the alpha value
    private float WarningAlpha;

    //Used to count how many times the warning logo has flashed
    private int count;


    void Start()
    {
        WarningAlphaUp = true;
    }
    

    private void Update()
    {
        if (TestSceneController.StartLevel == true)
        {
            transform.position += Vector3.left * (TestSceneController.globalEnemySpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine("EndOfStage");
        this.GetComponent<Collider2D>().enabled = false;
    }

    //END OF STAGE PROCESS
    //This flashes the end of stage warning then spawns the boss
    IEnumerator EndOfStage()
    {
        Warning.enabled = true;
        
        WarningAlpha = Warning.color.a;
        
        //Flash the warning logo 3 times (count variable used to control it)
        while(count != 3) {

        
            if (WarningAlphaUp)
            {
                WarningAlpha += (0.01f * 1.0f);
                Warning.color = new Color(1,1,1,WarningAlpha);
                
                if (WarningAlpha >= 1)
                {
                    WarningAlphaUp = false;
                    
                }

                yield return new WaitForSeconds(0.01f);
            }

            if (!WarningAlphaUp)
            {

                WarningAlpha -= (0.01f * 1.0f);
                Warning.color = new Color(1, 1, 1, WarningAlpha);

                if (WarningAlpha <= 0)
                {
                    WarningAlphaUp = true;
                    count++;
                }

                yield return new WaitForSeconds(0.01f);
            }

            //Once this has flashed 3 times, then spawn the boss!!
            if(count == 3)
            {
                Instantiate(Boss, new Vector3(25f, 0f, 0f), Quaternion.identity);
                Destroy(this.gameObject);
                yield return null;
            }
        }
    }
}
