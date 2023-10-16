using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space_Scroll_BG : MonoBehaviour
{
    //Used to multiply by player speed
    public float BG_Multiplier;

    //Variable used to store the player's speed
    private float PlaySpeed;

    //start and end of the BG position
    public float BG_Start_Pos;

    public float BG_End_Pos;
    
   // private void Start()
   // {
    //    PlaySpeed = Player.PlayerSpeed * BG_Multiplier;
   // }

    // Update is called once per frame
    void Update()
    {
       if(PlaySpeed != Player.PlayerSpeed * BG_Multiplier)
        {
            PlaySpeed = Player.PlayerSpeed * BG_Multiplier;
        }

       if (transform.position.x < BG_End_Pos)
       {
            transform.position = new Vector3(BG_Start_Pos, 0f, 1f);
       }

        transform.position -= new Vector3(PlaySpeed, 0f, 0f);
        /*
        //If the player is dashing
        if (Input.GetKey(KeyCode.Q) && !Player.DashRecharge || Input.GetKey("joystick button 0") && !Player.DashRecharge)
        {
            transform.position -= new Vector3((PlayDashSpeed / BG_Speed), 0, 0f);
        }
        else if (!Input.GetKey(KeyCode.Q) || !Input.GetKey("joystick button 0"))
        {
            transform.position -= new Vector3((PlaySpeed / BG_Speed), 0, 0f);
        }
        */
    }
}
