using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mvmt : MonoBehaviour
{
    //Used to store where the player is and where to slerp to. I don't want to
    //move the slerp with the changing player poss (yet, 05/07/21 - ZA)
    public Vector3 eMvmtPatt4EndPos;

    public bool eMvmtPatt4Slerp;


    //Mvmt 1 - Straight ahead
    public void eMvmt1(float speed)
    {
     
        transform.position += Vector3.left * (speed * Time.deltaTime);
    }

    //Mvmt 2 - Directions
    //Code take from - https://answers.unity.com/questions/1141555/move-a-object-along-a-vector.html
    public void eMvmt2(float speed)
    {
        transform.position -= (transform.position - new Vector3((-LevelGenerator.WorldSize.x - 2f), (LevelGenerator.WorldSize.y + 2f), 10f)).normalized * (speed);
    }

    public void eRotation(float RotSpeed)
    {
        Vector3 vectorToTarget = Player.PlayerPosition - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 180;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * RotSpeed);
    }


    //Mvmt 3 - Move enemy in SIN wave
    //Tutorial - https://www.youtube.com/watch?v=mFOi6W7lohk
    public void eMvmt3(float speed, float frequency, float amplitude, float ST)
    {
        //Get Current Position
        Vector3 CurrPos = transform.position;

        //Sin Frequency - speed of the wave moment
        frequency = 3f;

        //Sin amplitude - size of the wave moment
        amplitude = 0.5f;

        /* == NOTE - Must be Time.time, not Time.deltaTime!! ==*/
        float sin_wave = Mathf.Sin((Time.time - ST) * frequency) * amplitude;

        //Add the x movement to current x
        CurrPos.x += -1 * speed;
        //multiply the Y by the sin wave so the object doesn't lose it's initial Y pos
        CurrPos.y += -1 * speed * sin_wave;
        transform.position = CurrPos;
    }

    /* Mvmt 3, Version 2- Move enemy in half SIN wave then go straight ahead
     * What I wanted cause SLERP has slowdown at the end (05/07/21 - ZA)
     * If looking for where Mathf.Sin > 0, will move in a + y direction
     * If looking for where Mathf.Sin < 0, will move in a - y direction
     * this is in dependent of frequency and amplitude
     */
    public void eMvmt3_2(float speed, float frequency, float amplitude, float ST,bool phase2)
    {
        //Check to see if we should be moving in a SIN wave (phase 1)
        //or straight ahead (phase 2)
        if (Mathf.Sin(Time.time - ST) < 0f && !phase2)
        {
            phase2 = true;
        }

        //Phase 1 - move in SIN wave
        if (!phase2)
        {          
            Vector3 CurrPos = transform.position;

            //Sin Frequency - speed of the wave moment
            frequency = 1f;

            //Sin amplitude - size of the wave moment
            amplitude = 10f;

            /* == NOTE - Must be Time.time, not Time.deltaTime!! ==*/
            float sin_wave = Mathf.Sin((Time.time - ST) * frequency) * amplitude;

            //Add the x movement to current x, but only make the Y movement equal to current sine wave Position
            CurrPos.x += -1 * speed;
            CurrPos.y = sin_wave;
            transform.position = CurrPos;
        }

        else
        {
            transform.position += Vector3.left * (speed * Time.deltaTime);
        }
    }

    /*
     * // The higher the journey time value, the longer it takes to get from position 1 to position 2 (last value)
public void eMvmt4(float speed, Vector3 eMvmt4StartPos, Vector3 eMvmt4EndPos, float ST, float JT, bool Slerp)
{
    if(Slerp)
    {
        Vector3 centre = (eMvmt4StartPos + eMvmt4EndPos) * 0.5f;

        //This controls the arc width, the larger the Y value, the smaller the arc
        centre -= new Vector3(0f, 20f, 0f);
        Vector3 StartCentre = eMvmt4StartPos - centre;
        Vector3 EndCentre = eMvmt4EndPos - centre;
        float fracComplete = (Time.time - ST) / JT;
        transform.position = Vector3.SlerpUnclamped(StartCentre, EndCentre, 0.01f);
        transform.position += centre;
    }
    if(!Slerp)
    {
        transform.position += Vector3.left * speed;
    }      
}

    //OLD CODE FROM ENEMY 1
     Debug.Log("MOVE PATTERN 4");
                                                                                 Debug.Log("transform.position.x - Player.PlayerPosition.x = " + (transform.position.x - Player.PlayerPosition.x));
                                                                                 Debug.Log("pat4mvmt = " + pat4mvnt);*/
    /*              //Check trigger that means that when the enemy meets the player position -3, they go straight
                  int dismeasure = 3;
                  if ((transform.position.x - pat4endpos.x) > dismeasure)
                  {
                      //All this is SLERP stuff that is required. Refer the link above in the public variables declartion
                      Vector3 centre = (pat4startpos + pat4endpos);

                      //This controls the arc width
                      centre -= new Vector3(0f, 0.5f, 0f);
                      Vector3 StartCentre = pat4startpos - centre;
                      Vector3 EndCentre = pat4endpos - centre;
                      float fracComplete = (Time.time - startTime) / journeyTime;
                      transform.position = Vector3.Slerp(StartCentre, EndCentre, fracComplete);
                      transform.position += centre;
                  }

                  if (transform.position.x - pat4endpos.x <= dismeasure)
                  {
                      EnemyMovementPattern = 0;
                  }
                  break;

*/
}