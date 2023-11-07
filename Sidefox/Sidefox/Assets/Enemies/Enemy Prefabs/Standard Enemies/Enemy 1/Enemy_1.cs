using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_1 : MonoBehaviour
{


    //Scene Controller to refer to
    //[Header("This needs to be changed depending on the scene", order = 1)]
    //public GameObject CurrSceneController;
    //[Space(10, order = 2)]

    [Header("Unique Enemy Values", order = 3)]
    public Enemy_Base.eBaseValues e1;
    //Speed at which the enemy is moving
    public float e1Speed;

    //checks if the enemy should be floating towards player
    public bool RotateTowardsPlayer;
    [Space(10, order = 4)]

    [Header("Bullet Values", order = 5)]
    //public EnemyBullet bullClass;
    
    public GameObject enemybullet; // GameObject for 3D game
    public float BulletSpeed;
    public int eBulletDamage;
    public float eBaseBulletTimer;
    public float eBulletTimer;
    [Space(10, order = 6)]

    [Header("Movement Values", order = 7)]
    public Enemy_Mvmt e1m;
    public float e1RotationSpeed;
    /*== FOR ENEMY WAVE MOVEMENT ==*/
    //Used to control the size of the enemy wave
    public float e1e3Amp;
    //Used to control the speed of the enemy wave
    public float e1e3Freq;
    [Space(10, order = 8)]

    private Material defaultMat;

    public Material hitMat;

    //GameObject for the next enemy bullet
    

    //Set the enemy start position based on smovement pattern
    public int EnemyStartPos;

    //Set the enemy movement pattern
    public int EnemyMovementPattern;

    //Used to control the start time of the pattern 4 slerp angle. This needs to be set where ever we want to begin our SLERP movement
    private float startTime;

    //Used to calculate enemy health bars. This is the total of the enemies health
    [SerializeField]
    private float Overall_eHealth;

    //Enemy Health
    public float eHealth;

    //Bool that the enemy was hit
    public bool eHit;

    //spawn hit fragments when the enemy was hit by the bullet
    public GameObject eHit_Fragments;

    //total time the enemy shakes for
    [SerializeField]
    private float shakeAmt;

    //Use to set the enemy explosion animation
    [SerializeField]
    GameObject EnemyDeathExplosion;

    //Measures how far through the shake the enemy is
    [SerializeField]
    public float shakeTotalTime;

    //Enemies position
    public Vector3 ePosition;

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
    * *****************/

    //Calculates minimum and maximum distance the enemy can travel to reposition
    private float eRePosDistanceMin;

    private float eRePosDistanceMax;

    private float eRePosDistance;

    private bool eRePosition;

    private float eRePosTime;

    // Start is called before the first frame update

void Start()
    {
        //Set the enemy movement pattern
        e1m = this.gameObject.GetComponent<Enemy_Mvmt>();

        eRePosition = false;

        if (TestSceneController.globalEnemySpeed == 0)
        {
            TestSceneController.globalEnemySpeed = e1Speed;
        }


        eRePosDistanceMax = TestSceneController.SceneDimensions.y - (gameObject.GetComponent<Renderer>().bounds.size.y / 2);

        e1 = new Enemy_Base.eBaseValues(e1Speed, transform.position, RotateTowardsPlayer, e1RotationSpeed, false, eBaseBulletTimer);

        e1.EnemyMovementPattern = EnemyMovementPattern;

        //Set the enemy health bar canvas to the correct transform
        EnemyHealthBarCanvas = this.transform.GetChild(0);

        /**********************
        * NOTE: This will need to be updated when I'm testing other scenes. Need to figure out the
        * best way to handle this
        *********************/
        //ScreenX = CurrSceneController.GetComponent<TestSceneController>().SceneDimensions.x;
        //ChkOnScreenX = TestSceneController.SceneDimensions.x;

        //Ignore the collision between enemies
        Physics2D.IgnoreLayerCollision(8, 8);

       
        //Set the default material colour
        defaultMat = gameObject.GetComponent<SpriteRenderer>().material;

        //Set Overall eHealth to Health so the health bar works properly
        Overall_eHealth = eHealth;

        //Set enemy hit measure
        eHit = false;

        //Set enemy hit shake time
        shakeTotalTime = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {

        if (TestSceneController.StartLevel == true)
        {
            //    //Debug.Log("EMP = " + EnemyMovementPattern);

            //If the enemy is not on screen, have them move in a straight line and
            //continue to check to see if they're on screen yet

            //e1Speed = (Player.PlayerSpeed / 2);

            if (!e1.OnScreen)
            {
                e1m.eMvmt1(e1.Speed);
                if (transform.position.x < TestSceneController.SceneDimensions.x && this.GetComponent<Collider2D>().enabled == false)
                {
                    e1.OnScreen = true;
                    this.GetComponent<Collider2D>().enabled = true;
                }
            }

            //If the enemy is on screen, start their bullshit
            if (e1.OnScreen)
            {
                e1Fire();

                e1Movement();

                //Rotation check
                if (e1.RotateTowardsPlayer == true)
                {
                    e1m.eRotation(e1.eRotationSpeed);
                }

                //If the enemy in behind an object, move them out from behind it
                if (eBulletTimer <= (eBaseBulletTimer / 2) && transform.position.x > 0f && RotateTowardsPlayer == false)
                {

                    Debug.DrawRay(transform.position, Vector2.left * 2f, Color.green);
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
            }

            //This is used to control how much to show in the enemy health bar
            HealthBar.fillAmount = (eHealth / Overall_eHealth);

            if (Input.GetKey(KeyCode.H))
            {
                StartCoroutine(HitFlash());
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
        }
    }

    private void RepositionFromCover(float distance)
    {

        float lerpDuration = 1f;

        float timeElapsed = Time.deltaTime / lerpDuration;

        Vector3 newPos = new Vector3(transform.position.x, distance, transform.position.z);

        //Debug.Log("Current Pos = " + transform.position + " New Pos = " + newPos);

        transform.position = Vector3.Lerp(transform.position, newPos, timeElapsed);
        //transform.position += new Vector3(0f, -2f, 0f) * Speed;
    }


    //when the enemy is off screen, destory it
    private void OnBecameInvisible()
    {
        if (transform.position.x < Player.PlayerPosition.x)
        {
            Destroy(this.gameObject);

            Score.TotalEnemiesKilled -= 1;

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

            Instantiate(eHit_Fragments, new Vector3(collision.gameObject.transform.position.x - 2f, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z), Quaternion.Euler(0f, -90f, 0f));

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

            if (collision.gameObject.layer == 12)
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

    private void e1Fire()
    {
        e1.eBulletTimer -= Time.deltaTime;

        if (e1.eBulletTimer <= 0)
        {                      
            GameObject bullet = Instantiate(enemybullet, new Vector3(transform.position.x - 2, transform.position.y, transform.position.z), transform.rotation);

            bullet.GetComponent<EnemyBullet>().bullDamage = 10f;

            bullet.GetComponent<EnemyBullet>().bullSpeed = 10f;

            bullet.GetComponent<EnemyBullet>().eRotationStatus = RotateTowardsPlayer;

            // float rot = Mathf.Atan2(-transform.position.y,-transform.position.x) * Mathf.Rad2Deg;

            // bullet.GetComponent<EnemyBullet>().bullRotation = rot + 90f;

            e1.eBulletTimer = e1.eBaseBulletTimer;
        }
    }

    private void e1Movement()
    {

            switch (e1.EnemyMovementPattern)
            {
                //Movement Pattern 1 - Straight Ahead
                case 1:
                    e1m.eMvmt1(e1.Speed);
                    break;

                //Movement Pattern 2 - Diagonal
                case 2:
                    e1m.eMvmt2(e1.Speed);
                    break;

                //Movement Pattern 3 - SIN wave
                case 3:
                    if (e1.eMvmtStartTime == 0)
                    {
                        e1.eMvmtStartTime = Time.time;
                    }
                    e1m.eMvmt3(e1.Speed, e1e3Freq, e1e3Amp, e1.eMvmtStartTime);
                    break;

                //Movement Pattern 4 - half SINE wave then straight ahead
                case 4:
                    if (e1.eMvmtStartTime == 0)
                    {
                        e1.eMvmtStartTime = Time.time;
                    }

                    e1m.eMvmt3_2(e1.Speed, e1e3Amp, e1e3Freq, e1.eMvmtStartTime, false);
                    break;

                default:
                    e1m.eMvmt1(e1.Speed);
                    break;
        }
    }


}//END OF CLASS