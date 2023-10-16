using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_Base : MonoBehaviour
{

    public class eBaseValues
    {
        //MOMVEMENT VARIABLES//

        //Set the enemy start position based on movement pattern
        public int EnemyStartPos;

        //Speed at which the enemy is moving
        public float Speed;

        //Enemies position
        public Vector3 ePosition;

        //Check to see if the player is on screen
        public bool OnScreen;

        //Set the enemy movement pattern
        public int EnemyMovementPattern;

        //checks if the enemy should be floating towards player
        public bool RotateTowardsPlayer;

        //speed at which the enty
        [Header("control how fast the enemty rotates. Smaller number is slower")]
        public float eRotationSpeed;

        //Used to control the start time of the pattern 4 slerp angle. This needs to be set where ever we want to begin our SLERP movement
        private float startTime;

        //BULLET VARIABLES//

        //Used to countdown to when the next bullet is to be fired
        public float eBulletTimer;

        //Used to reset the bullet timer
        public float eBaseBulletTimer;

        //Bullet Damage
       // public float eBulletDamage;

        //Bullet Pattern
      //  public float eBulletPattern;

        //Bullet Speeds
      //  public float eHorizontalBulletSpeed;

      //  public float eVerticalBulletSpeed;

        //HEALTH VARIABLES//
        //Used to calculate enemy health bars. This is the total of the enemies health
        public float Overall_eHealth;

        //Enemy Health
        public float eHealth;


        //Bool that the enemy was hit
        public bool eHit;

        /* Used to control when to start moving in a pattern. Applies to -
         * Mvmt 3 - SIN movement
         * Mvmt 4 - Slerp then straight
         */
        public float eMvmtStartTime;

        //Measures how far through the shake the enemy is
        [SerializeField]
        public float shakeTotalTime;

        //Check for collision
        private bool CollisionCheck;

        public eBaseValues(float spe, Vector3 ePos, bool rot, float eRotSpe, bool OnScrn, float bulltim)
        {
            Speed = spe;

            ePosition = ePos;

            RotateTowardsPlayer = rot;

            eRotationSpeed = eRotSpe;

            //No enemies should ever start on screen
            OnScreen = OnScrn;

            eHit = false;

            eBulletTimer = bulltim;

            eBaseBulletTimer = bulltim;

          //  eBulletDamage = bulldam;

            //eBulletPattern = bullpat;

            //eHorizontalBulletSpeed = bullspe;

        }
    }
}

/*

    
    
    

    //Scene Controller to refer to
    [Header("This needs to be changed depending on the scene", order = 1)]
    public GameObject CurrSceneController;
    [Space(20, order = 2)]

    

    //Size of the screen width
    public float ScreenX;

//Used to control the size of the enemy wave
private float amplitude;

    private Material defaultMat;

    public Material hitMat;

    //Used to see when the enemy should fire a bullet next
    public float _EnemyBulletCountDown;

    //GameObject for the next enemy bullet
    public GameObject enemybullet; // GameObject for 3D game
        
    

    //spawn hit fragments when the enemy was hit by the bullet
    public GameObject eHit_Fragments;

    //total time the enemy shakes for
    [SerializeField]
    private float shakeAmt;

    //Use to set the enemy explosion animation
    [SerializeField]
    GameObject EnemyDeathExplosion;

    

    //All of the below are used for the pattern 4 slerp movement. Information here -
    // https://docs.unity3d.com/ScriptReference/Vector3.Slerp.html
    public Vector3 pat4startpos;

    public Vector3 pat4endpos;

    //Get the bottom of the screen in world unit locations
   // Vector3 BottomS;

    //Starting point for diagonal movement
    private float YStartPoint;

    //Used to have pattern movement 4 target this position to not have the enemy keep juttering around
    public static Vector3 pat4playertargetpos;

    //How long it will take the enemy to move somewhere? <- Need to confirm
    public float journeyTime = 5.0f;

    [Header("Enemy Health UI")] // Creates subset of elements in Inspector
    //Enemy Health Bar
    public Image HealthBar;

    //Used to store the length of the health bar
    public Transform EnemyHealthBarCanvas;

    //Collison check for being behind environment
    private RaycastHit2D hit;

     /*******************
     * Used to see how to move the player around cover
     * ****************

    //Calculates minimum and maximum distance the enemy can travel to reposition
    private float eRePosDistanceMin;

    private float eRePosDistanceMax;

    private float eRePosDistance;

    private bool eRePosition;

    private float eRePosTime;

   // Start is called before the first frame update
    void Start()
    {
        //Set the enemy health bar canvas to the correct transform
        EnemyHealthBarCanvas = this.transform.GetChild(0);

        //Set all the enemy checks to false
        OnScreen = false;

        CollisionCheck = false;

        eRePosition = false;

        eRePosDistanceMax = TestSceneController.SceneDimensions.y - (gameObject.GetComponent<Renderer>().bounds.size.y/2);
        

        

        /**********************
        * NOTE: This will need to be updated when I'm testing other scenes. Need to figure out the
        * best way to handle this
        ********************
        //ScreenX = CurrSceneController.GetComponent<TestSceneController>().SceneDimensions.x;
        ScreenX = TestSceneController.SceneDimensions.x;

        //Ignore the collision between enemies
        Physics2D.IgnoreLayerCollision(8, 8);


        _EnemyBulletCountDown = bulletcountdown;

        //Calculate the maximum size of the wave
        amplitude = Random.Range(1, (int)LevelGenerator.WorldSize.y);
        //Debug.Log("Amplitude = " + amplitude);
        //Debug.Log("GameObeject Height = " + gameObject.GetComponent<Renderer>().bounds.size);

        //Set the default material colour
        defaultMat = gameObject.GetComponent<SpriteRenderer>().material;

       //Set Overall eHealth to Health so the health bar works properly
       Overall_eHealth = eHealth;

        //Set the enemy bullet speed so it's always slight faster than the enemy
        enemybullet.GetComponent<EnemyBullet>().HoriBulletSpeed = Speed + 0.05f;

        enemybullet.GetComponent<EnemyBullet>().VertBulletSpeed = 0f;

        //Set enemy hit measure
        eHit = false;

        //Set enemy hit shake time
        shakeTotalTime = 0.25f;
    }
   
    // Update is called once per frame
    void Update()
    {

        if (RotateTowardsPlayer)
        {
            Vector3 vectorToTarget = Player.PlayerPosition - transform.position;
            float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 180;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            //transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 1);
            transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * 5);
        }

        //We just want to move the enemy until they are on screen. Once on screen, then
        //we do some funky stuff
        if (!OnScreen)
        {
            //Move the enemy until they're on screen
            transform.position += Vector3.left * Speed;

            //If the enemy is on screen, start their bullshit
            if (transform.position.x < ScreenX && this.GetComponent<Collider2D>().enabled == false)
            {
                OnScreen = true;
                //  Debug.Log("TURN ON COLLDER");
                this.GetComponent<Collider2D>().enabled = true;
            }
        }

        if (OnScreen)
        {

            //    Debug.DrawRay(transform.position, Vector2.left * 2f, Color.green);

            //Start the contdown until the enemy can shoot
            bulletcountdown -= Time.deltaTime;

            //If the enemy in behind an object, move them out from behind it
            if (bulletcountdown <= (_EnemyBulletCountDown / 2) && transform.position.x > 0f && RotateTowardsPlayer == false)
            {
                //See if there's any environment in front of the player
                //Vector3 forward = new Vector3(,0f,0f);
                hit = Physics2D.Raycast(transform.position, Vector3.left, 10);
                Debug.DrawRay(transform.position, Vector3.left * 10, Color.green);
            }

                //if they have hit the the environment
                if (hit.collider != null && hit.collider.tag == "Environment" && eRePosition == false)
                {
                    GameObject HitObj = hit.collider.gameObject;
                    eRePosDistanceMin = Mathf.Abs(HitObj.GetComponent<Renderer>().bounds.size.y / 2) - HitObj.transform.position.y + (GetComponent<Renderer>().bounds.size.y / 2);

                    if (transform.position.y <= 0 && eRePosDistance == 0)
                        {
                            eRePosDistance = transform.position.y + Random.Range(eRePosDistanceMin, eRePosDistanceMax);
                        }

                    if (transform.position.y > 0 && eRePosDistance == 0)
                    {
                        eRePosDistance = (transform.position.y + Random.Range(eRePosDistanceMin, eRePosDistanceMax)) * -1.0f;
                    }

                Debug.Log("eRePosDistanceMin = " + eRePosDistanceMin);
                Debug.Log("eRePosDistanceMax = " + eRePosDistanceMax);
                Debug.Log("eRePosDistance = " + eRePosDistance);


                eRePosition = true;
             }



            if (eRePosition)
            {
                RepositionFromCover(eRePosDistance);
            }


            //If the enemy gets hit with a bullet and they are below a certain amount
            //of health, shake them
            if (eHit)
            {
                if (shakeTotalTime > 0)
                {
                    transform.position = transform.position + Random.insideUnitSphere * 0.25f;
                    shakeTotalTime -= Time.deltaTime;
                }
                if (shakeTotalTime <= 0)
                {
                    //this.transform.position = ePosition;
                    eHit = false;
                    shakeTotalTime = shakeAmt;
                }

               
            }
            

            

            //If the enemy is to far away from the screen, then just move them forward,
            //once they get close enough to the screen, then start them on their movement pattern
            if (transform.position.x > (LevelGenerator.WorldSize.x + EnemyHealthBarCanvas.GetComponent<RectTransform>().sizeDelta.x))
            {
                transform.position += Vector3.left * Speed;

                if (EnemyMovementPattern == 4)
                {
                    startTime = Time.time;
                }
            }
            else
            {
                //=============================//
                //== ENEMY MOVEMENT PATTERNS ==//
                //=============================//

                switch (EnemyMovementPattern)
                {
                    /*== MOVE ENEMY STRAIGHT AHEAD ==
                    case 0:
                        //MOVEMENT PATTERN
                        transform.position += Vector3.left * Speed;
                        break;

                    /*== MOVE ENEMY 1 CORNER TO THE OTHER == //Rotate towards player does just that
                    case 1:
                        //Code take from - https://answers.unity.com/questions/1141555/move-a-object-along-a-vector.html

                        transform.position -= (transform.position - new Vector3((-LevelGenerator.WorldSize.x - 2f), (-LevelGenerator.WorldSize.y - 2f), 10f)).normalized * Speed;
                        //transform.position += (transform.position - Camera.main.ViewportToWorldPoint(new Vector3(0f, 0f, 10f))).normalized * Speed;

                        //Debug.Log("NORMALISED = " + (transform.position - Camera.main.ViewportToWorldPoint(new Vector3(-1f, 2f, 10f))).normalized);
                        //RotateTowardsPlayer = true;
                        break;

                    case 2:
                        transform.position -= (transform.position - new Vector3((-LevelGenerator.WorldSize.x - 2f), (LevelGenerator.WorldSize.y + 2f), 10f)).normalized * Speed;
                        //OLD CODE
                        //transform.position -= (transform.position - Camera.main.ViewportToWorldPoint(new Vector3(-1f, 1f, 10f))).normalized * Speed;
                        break;



                    /*== MOVE ENEMY IN SIN WAVE ==
                    //Tutorial - https://www.youtube.com/watch?v=mFOi6W7lohk
                    case 3:
                        //Get Current Position
                        Vector3 CurrPos = transform.position;

                        //Sin Frequency - speed of the wave moment
                        float frequency = 3f;

                        //Sin amplitude - size of the wave moment
                        float amplitude = 5f;

                        /* == NOTE - Must be Time.time, not Time.deltaTime!! ==                        float sin_wave = Mathf.Sin((Time.time - startTime) * frequency) * amplitude;

                        //Add the x movement to current x, but only make the Y movement equal to current sine wave Position
                        CurrPos.x += -1 * Speed;
                        CurrPos.y = sin_wave;
                        transform.position = CurrPos;
                        break;

                    case 4:
                        /*
                        Debug.Log("MOVE PATTERN 4");
                        Debug.Log("transform.position.x - Player.PlayerPosition.x = " + (transform.position.x - Player.PlayerPosition.x));
                        Debug.Log("pat4mvmt = " + pat4mvnt);
                        //Check trigger that means that when the enemy meets the player position -3, they go straight
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

                    default:
                        transform.position += Vector3.left * Speed;
                        break;
                }
            }


            //This is used to control how much to show in the enemy health bar
            HealthBar.fillAmount = (eHealth / Overall_eHealth);

            //If the enemy is in the screen view
            if (/*(transform.position.x + (gameObject.GetComponent<Renderer>().bounds.size.x / 2)) < CurrSceneController.ScreenDimensions.x ||
                (transform.position.x + (gameObject.GetComponent<Renderer>().bounds.size.x / 2)) < SideScrollingSceneController.ScreenDimensions.x)
            {
                //When the timer has reached 0, shoot the enemy
                if (bulletcountdown <= 0)
                {
                    bulletcountdown = _EnemyBulletCountDown;
                    GameObject _eBull = Instantiate(enemybullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), transform.rotation);
                    //Instantiate(EnemyBullet, new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity);
                }

                // Debug.Log("transform.position.x + (gameObject.GetComponent<Renderer>().bounds.size.x / 2) = " + transform.position.x + (gameObject.GetComponent<Renderer>().bounds.size.x / 2));
                //this.gameObject.GetComponent<CapsuleCollider>().enabled = true; // used for 3D collision

                this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }

            if (Input.GetKey(KeyCode.H))
            {
                StartCoroutine(HitFlash());
            }
        }

    }

    private void RepositionFromCover(float distance)
    {

        float lerpDuration = 1f;

        float timeElapsed = Time.deltaTime / lerpDuration;

        Vector3 newPos = new Vector3(transform.position.x, distance, transform.position.z);

        //Debug.Log("Current Pos = " + transform.position + " New Pos = " + newPos);

        transform.position = Vector3.Lerp(transform.position,newPos ,timeElapsed);
        //transform.position += new Vector3(0f, -2f, 0f) * Speed;
    }


    //when the enemy is off screen, kill the player
    private void OnBecameInvisible()
    {
        if (transform.position.x < Player.PlayerPosition.x)
        {
            Destroy(this.gameObject);
        }
    }

    //Make the enemy flash
    public IEnumerator HitFlash()
    {
        
        GetComponent<SpriteRenderer>().material = hitMat;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material = defaultMat;
    }

    //When the bullet collides with 
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11 && GetComponent<Renderer>().isVisible || collision.gameObject.layer == 12 && GetComponent<Renderer>().isVisible)
        {

          
            //If an enemy is hit, take life away from them and shake them
            ePosition = collision.gameObject.transform.position;

           

            Instantiate(eHit_Fragments, new Vector3(collision.gameObject.transform.position.x - 2f, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z), Quaternion.Euler(0f,-90f,0f));

            StartCoroutine(HitFlash());

            //If the combo is zero, then make the text appear

            UI_Controller.ComboCounter += 1;
            if (UI_Controller.ComboCountdownTimer < 1f)
            {
                UI_Controller.ComboCountdownTimer = 1f;
            }

            if (collision.gameObject.layer == 11)
            {
                eHealth -= collision.gameObject.GetComponent<PlayerBullet>().pBullDamage;
            }

            if(collision.gameObject.layer == 12)
            {
                eHealth -= collision.gameObject.GetComponent<PlayerHomingMissile>().pSecondaryDamage;
            }

            //if the players health is left than half, SHAKE EM!
            if (eHealth < (Overall_eHealth / 2))
            {
                eHit = true;
            }

            //Get rid of the bullet object
            Destroy(collision.gameObject);
        }

        if (eHealth <= 0)
        {
          //  StartCoroutine(HitFlash());
          //  transform.position = transform.position + Random.insideUnitSphere * 0.25f;
            Destroy(this.gameObject);
            Instantiate(EnemyDeathExplosion, collision.transform.position, Quaternion.Euler(0f, 0f, 0f));
            Score.ScoreCount += 10;
        }
    }
}
*/