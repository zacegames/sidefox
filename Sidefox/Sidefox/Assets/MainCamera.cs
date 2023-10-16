using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    //How long should the screen shake for
    [SerializeField]
    private float cShakeTotalTime;
    
    //How long the screen has left to shake
    private float cShakeTime;
    
    // Start is called before the first frame update
    void Start()
    {
        cShakeTime = cShakeTotalTime;
    }

    // Update is called once per frame
    void Update()
    {
        //If the player has collided with an enemy, shake the screen

        if (Player.pHit)
        {
            
            if (cShakeTime > 0)
            {
                transform.position = transform.position + Random.insideUnitSphere * 0.75f;
                cShakeTime-= Time.deltaTime;
            }
            if (cShakeTime <= 0)
            {
                transform.position = new Vector3(0f, 0f, -10f);
                //this.transform.position = ePosition;
                Player.pHit = false;
                cShakeTime = cShakeTotalTime;
            }
        }
    }
}
