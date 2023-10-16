using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Controller : MonoBehaviour
{
    [Header("Combo Counter Variables")]
    public static int ComboCounter;

    public Text ComboCounterText;

    //How long to display the text for if the player hasn't hit anything
    public static float ComboCountdownTimer;

    //Used to store the default text color
    public Color ComboCounterTextColor;
    
    // Start is called before the first frame update
    void Start()
    {
        ComboCounterText.gameObject.SetActive(false);
        ComboCounterTextColor = ComboCounterText.color;
        ComboCountdownTimer = 3;
    }

    // Update is called once per frame
    void Update()
    {

        //Controls for making Combo Counter appear and disappear
        //Show it if the player has started a combo
        if (ComboCounterText.gameObject.activeSelf == false && ComboCounter > 0)
        {
            ComboCounterText.gameObject.SetActive(true);
        }

        if(ComboCounterText.gameObject.activeSelf == true)
        {
            ComboCounterText.text = "Combo! " + ComboCounter.ToString();
            
            //Fade the text if the player has not hit an enemy in a while
            if (ComboCounterText.color.a > 0.0f)
            {
              //  Debug.Log("ComboCountdownTimer = " + (ComboCountdownTimer -= (Time.deltaTime / 3)));
                ComboCounterText.color = new Color(ComboCounterText.color.r, ComboCounterText.color.g, ComboCounterText.color.b, ComboCountdownTimer -= (Time.deltaTime / 3));
            }

            //If the combo counter is dead, restart it
            if (ComboCounterText.color.a <= 0.0f)
            {
                if(ComboCounter > TestSceneController.highestComboCount)
                {
                    ComboCounter = TestSceneController.highestComboCount;
                }
                ComboCounter = 0;
            }
        }
        //End it if the player is hit by and enemy bullet
        if(ComboCounterText.gameObject.activeSelf == true && ComboCounter == 0)
        {
            ComboCounterText.gameObject.SetActive(false);
            ComboCounter = 0;
            ComboCounterText.color = ComboCounterTextColor;

        }

    }
}