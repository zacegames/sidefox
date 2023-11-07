using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_2 : MonoBehaviour
{

    //MOVEMENT VARIABLES//
    //variables to change in testing
    public float e2Speed;

    //used to control what type of movement the enemy will use
    public int eMvmtPattern;

    //used to control if this enemy should be roatating towards player
    public bool eRotation;

    //used to control the enemy rotation speed (if the enemy should be rotating)
    public float eRotationSpeed;

    //BULLET VARIABLES//

    //control the bullet speed
    public float eHoriBulletSpeed;

    public float eVertBulletSpeed;

    //control the bullet pattern
    public int eBulletPattern;

    //Damage to attach to the bullet
    public int eBulletDamage;

    //Bullet spawner
    public float eBulletTimer;

    //Bullet game object
    public GameObject eBulletObject;
    
    //control the bullet spawn rate

    public Enemy_Base.eBaseValues e2;

    public Enemy_Mvmt e2m;

    public EnemyBullet eBullet;

    [Header("Enemy Health UI")] // Creates subset of elements in Inspector
    //Enemy Health Bar
    public Image HealthBar;

    //Used to store the length of the health bar
    public Transform EnemyHealthBarCanvas;


    // Start is called before the first frame update
    void Start()
    {
       // e2Speed = 0.03f;

        e2 = new Enemy_Base.eBaseValues(e2Speed,transform.position, eRotation, eRotationSpeed, false,eBulletTimer);

        e2m = this.gameObject.GetComponent<Enemy_Mvmt>();

        eBullet = eBulletObject.gameObject.GetComponent<EnemyBullet>();

        //eBullet.eSetBulletValues(eHoriBulletSpeed, eVertBulletSpeed, eBulletPattern, eBulletDamage, e2Speed, false, transform.rotation);

        e2.EnemyMovementPattern = eMvmtPattern;

        e2m.eMvmtPatt4Slerp = true;

        e2.Overall_eHealth = 100;

        e2.eHealth = 100;

        //Set the enemy health bar canvas to the correct transform
        EnemyHealthBarCanvas = this.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("On Screen = " + e2.OnScreen);

        if (!e2.OnScreen)
        {
            e2m.eMvmt1(e2.Speed);
            if (transform.position.x < TestSceneController.SceneDimensions.x && this.GetComponent<Collider2D>().enabled == false)
            {
                e2.OnScreen = true;
                this.GetComponent<Collider2D>().enabled = true;
            }
        }

        //If the enemy is on screen, start their bullshit
        if (e2.OnScreen)
        {
            eMovement();
            eFire();

            //Rotation check
            if (e2.RotateTowardsPlayer == true)
            {
                e2m.eRotation(e2.eRotationSpeed);
            }
        }

        //This is used to control how much to show in the enemy health bar
        HealthBar.fillAmount = (e2.eHealth / e2.Overall_eHealth);
        /*

        if (e2.eHit)
        {
            Destroy(this.gameObject);
        }*/
    }

    /* == MOVEMENT CHECKS == */
    private void eMovement()
    {

        switch (e2.EnemyMovementPattern)
        {
            //Movement Pattern 1 - Straight Ahead
            case 1:
                e2m.eMvmt1(e2.Speed);
                break;

            //Movement Pattern 2 - Diagonal
            case 2:
                e2m.eMvmt2(e2.Speed);
                break;

            //Movement Pattern 3 - SIN wave
            case 3:
                if (e2.OnScreen)
                {
                    if (e2.eMvmtStartTime == 0)
                    {
                        e2.eMvmtStartTime = Time.time;
                    }

                    e2m.eMvmt3(e2.Speed, 0f, 0f, e2.eMvmtStartTime);
                }
                break;

            //Movement Pattern 4 - half SINE wave then straight ahead
            case 4:
                if (e2.OnScreen)
                 {
                     if (e2.eMvmtStartTime == 0)
                     {
                         e2.eMvmtStartTime = Time.time;
                     }

                     e2m.eMvmt3_2(e2.Speed, 0f, 0f, e2.eMvmtStartTime, false);
                 }
            break;

            default:
                e2m.eMvmt1(e2.Speed);
                break;

        }
    }

    private void eFire()
    {
        e2.eBulletTimer -= Time.deltaTime;

        if(e2.eBulletTimer <= 0)
        {
            
            Instantiate(eBulletObject, new Vector3(transform.position.x -2, transform.position.y, transform.position.z), Quaternion.identity);

            e2.eBulletTimer = e2.eBaseBulletTimer;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11 && GetComponent<Renderer>().isVisible || collision.gameObject.layer == 12 && GetComponent<Renderer>().isVisible)
        {
            e2.eHit = true;

            UI_Controller.ComboCounter += 1;
            if (UI_Controller.ComboCountdownTimer < 1f)
            {
                UI_Controller.ComboCountdownTimer = 1f;
            }

            if (collision.gameObject.layer == 11)
            {
                e2.eHealth -= collision.gameObject.GetComponent<PlayerBullet>().pBullDamage;
            }

            if (collision.gameObject.layer == 12)
            {
                e2.eHealth -= collision.gameObject.GetComponent<PlayerHomingMissile>().pSecondaryDamage;
            }

            //Get rid of the bullet object
            Destroy(collision.gameObject);


            if (e2.eHealth <= 0)
            {
                Destroy(this.gameObject);
                Score.ScoreCount += 10;
            }
        }
    }

    //when the enemy is off screen, kill the player
    private void OnBecameInvisible()
    {
        if (transform.position.x < Player.PlayerPosition.x)
        {
            Destroy(this.gameObject);
        }
    }
}
