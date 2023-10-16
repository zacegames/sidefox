using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class Boss_1 : MonoBehaviour
{
    //Boss sprite width 
    private float bSpriteWidth;

    //current scene controller
    public GameObject CurrSceneController;

    [Header("Enemy Health UI Variables")]

    //Boss overall health
    [SerializeField]
    private float bOverall_Health;

    //Current Health of the boss
    private float bHealth;

    // Creates subset of elements in Inspector
    //Enemy Health Bar
    public Image bHealthBar;

    //Used to store the length of the health bar
    public Transform bHealthBarCanvas;

    //Boss speed
    [SerializeField]
    private float bSpeed;

    //Set the time delay for the boss bullet delay
    [SerializeField]
    private float bBulletGenDelay;

    //The boss bullet that he shoots
    public GameObject bBullet;

    public EnemyBullet beBullet;

    //Used to set what time of bullet pattern the boss should fire. Used to pass
    //through new bullet pattern
    private int bCurrentBulletPattern;

    //used to store the previous boss bullet pattern
    private int bPrevBulletPattern;

    [Header("The Time.Deltatime is done in the bullet class. Set as whole number")]
    //The boss bullet speed
    public float bBulletSpeed;

    //The gameObject at the top of the boss (used to emit bullets)
    private Transform bBulletPosTop;

    //The gameObject at the bottom of the boss (used to emit bullets)
    private Transform bBulletPosBtm;

    //Used to ensure that the bullet moves and rotates independently of the boss
    Vector3 bBulletLocalPos;

    Quaternion bBulletLocalRotation;

    //Used to time when bullters spawn
    private float bBulletSpawn;

    //Used to calculate when the boss will reach the edge of the sceen
    //on the Y component
    private float bYScreenEdge;

    //Boss is moving in the Y in the negative
    private bool bUp;

    //Default material is for after enemy hit flash, putting the standard material back on the boss
    private Material defaultMat;

    //This is used for feedback that the player has hit the boss. Boss flashes this colour when hit
    public Material bHitMat;

    //Use to set the enemy explosion animation
    [SerializeField]
    GameObject EnemyDeathExplosion;

    // Start is called before the first frame update
    void Start()
    {
        
        bHealth = bOverall_Health;

        defaultMat = gameObject.GetComponent<SpriteRenderer>().material;

        //Set the boss health bar canvas to the correct transform
        bHealthBarCanvas = this.transform.GetChild(2);

        beBullet = bBullet.GetComponent<EnemyBullet>();

        //Horizontal Speed, Vertical Speed, Pattern, Damage, bullet spawn speed
        //Do not time.Delta time needs to be configured here

        Debug.Log("Speeds - " + (bBulletSpeed + bSpeed));

        beBullet.eSetBulletValues(bBulletSpeed, bBulletSpeed, 1, 20, 2, false, transform.rotation);


        bSpeed = bSpeed * Time.deltaTime;

        //Debug.Log("bBulletPosTop = " + bBulletPosTop);

        //Debug.Log("bBulletPosBtm = " + bBulletPosBtm);

        //Debug.Log("Screen Edge Y = " + bYScreenEdge);

        //bBulletSpeed = -0.05f;

        bUp = true;

        bSpriteWidth = Mathf.Round(GetComponent<SpriteRenderer>().bounds.size.y / 2);

        bYScreenEdge = TestSceneController.SceneDimensions.y - bSpriteWidth;

        bBulletPosTop = this.gameObject.transform.GetChild(0);

        bBulletPosBtm = this.gameObject.transform.GetChild(1);

        bBulletSpawn = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Move the boss up the screen (+ Y)
        if (transform.position.y < bYScreenEdge && bUp)
        {
            transform.position += Vector3.up * bSpeed;
        }
        if (transform.position.y >= bYScreenEdge && bUp)
        {
            bUp = false;
        }

        bHealthBar.fillAmount = (bHealth / bOverall_Health);

        //move the boss down the screen (-Y)
        if (transform.position.y > -bYScreenEdge && !bUp)
        {
            transform.position += Vector3.down * bSpeed;
        }

        if (transform.position.y <= -bYScreenEdge && !bUp)
        {
            bUp = true;
        }



        //If the boss has survived
        if (Time.time > (bBulletSpawn + bBulletGenDelay))
        {
           // Debug.Log("FIRE BULLETS");
            //Randomise the boss bullet pattern
          //  while (bCurrentBulletPattern == bPrevBulletPattern)
          //  {
                bCurrentBulletPattern = Random.Range(1, 3);
                //Debug.Log("bCurrentBulletPattern = " + bCurrentBulletPattern);
          //  }

            BulletTime();

            //set this so it changes the bullet pattern
            bPrevBulletPattern = bCurrentBulletPattern;
            bBulletSpawn = Time.time;
        }
    }

    void BulletTime()
    {

        switch (bCurrentBulletPattern)
        {
            case 1: //Two bullet straight ahead
                bBulletLocalPos = bBulletPosTop.transform.position;

                bBulletLocalRotation = bBulletPosTop.rotation;

                beBullet.eVertBulletSpeed = 0f;

                Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);

                bBulletLocalPos = bBulletPosBtm.transform.position;

                bBulletLocalRotation = bBulletPosBtm.rotation;

                Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);
                break;

            case 2: //top then delayed bottom shot
                beBullet.eBulletPattern = 1;
                StartCoroutine("bBulletTopBottomDelay");
                break;
            //Triple shot
            case 3:
                //Set deafault position and location
                bBulletLocalPos = bBulletPosTop.transform.position;

                bBulletLocalRotation = bBulletPosTop.rotation;

                // beBullet.eHoriBulletSpeed = bBulletSpeed;

                //Top position bullets
                // beBullet.eVertBulletSpeed = 0f;

                beBullet.eBulletPattern = 1;

                Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);

                beBullet.eBulletPattern = 2;

                Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);

                beBullet.eBulletPattern = 3;

                Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);

                //Bottom position bullets
                bBulletLocalPos = bBulletPosBtm.transform.position;

                bBulletLocalRotation = bBulletPosBtm.rotation;

                Debug.Log(bBulletLocalRotation);

                beBullet.eBulletPattern = 1;

                Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);

                beBullet.eBulletPattern = 2;

                Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);

                beBullet.eBulletPattern = 3;

                Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);
                break;

            //Triple shot with delay between each bullet
            case 4:
                StartCoroutine("bBulletSpreadDelay");
                break;

            default:
                bBulletLocalPos = bBulletPosTop.transform.position;

                bBulletLocalRotation = bBulletPosTop.rotation;

                Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);

                bBulletLocalPos = bBulletPosBtm.transform.position;

                bBulletLocalRotation = bBulletPosBtm.rotation;

                Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);
                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 11 && GetComponent<Renderer>().isVisible || collision.gameObject.layer == 12 && GetComponent<Renderer>().isVisible)
        {


            //If an enemy is hit, take life away from them and shake them
            //ePosition = collision.gameObject.transform.position;

            //Instantiate(eHit_Fragments, new Vector3(collision.gameObject.transform.position.x - 2f, collision.gameObject.transform.position.y, collision.gameObject.transform.position.z), Quaternion.Euler(0f, -90f, 0f));

            StartCoroutine(HitFlash());

            //If the combo is zero, then make the text appear
            UI_Controller.ComboCounter += 1;
            if (UI_Controller.ComboCountdownTimer < 1f)
            {
                UI_Controller.ComboCountdownTimer = 1f;
            }

            if (collision.gameObject.layer == 11)
            {
                bHealth -= collision.gameObject.GetComponent<PlayerBullet>().pBullDamage;
            }

            if (collision.gameObject.layer == 12)
            {
                bHealth -= collision.gameObject.GetComponent<PlayerHomingMissile>().pSecondaryDamage;
            }

            //Get rid of the bullet object
            Destroy(collision.gameObject);
        }

        if (bHealth <= 0)
        {
            
            Instantiate(EnemyDeathExplosion, collision.transform.position, Quaternion.Euler(0f, 0f, 0f));
            CurrSceneController.GetComponent<TestSceneController>().endGame();
            Destroy(this.gameObject);
        }
    }

    public IEnumerator HitFlash()
    {

        GetComponent<SpriteRenderer>().material = bHitMat;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().material = defaultMat;
    }

    IEnumerator bBulletTopBottomDelay()
    {
        //beBullet.eHoriBulletSpeed; = bBulletSpeed;

        for (int i = 0; i <= 1; i++)
        {
            Debug.Log("i = " + i);

            if (i == 0)
            {
                //Set deafault position and location
                bBulletLocalPos = bBulletPosTop.transform.position;
                bBulletLocalRotation = bBulletPosTop.rotation;
            }

            if (i == 1)
            {
                //Set deafault position and location
                bBulletLocalPos = bBulletPosBtm.transform.position;
                bBulletLocalRotation = bBulletPosBtm.rotation;
            }

            Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);
            yield return new WaitForSeconds(.4f);
            //yield return null;            
        }
    }

    IEnumerator bBulletSpreadDelay()
    {
        //beBullet.eHoriBulletSpeed; = bBulletSpeed;

        for (int i = 0; i <= 5; i++)
        {
            Debug.Log("i = " + i);

            if (i <= 2)
            {
                //Set deafault position and location
                bBulletLocalPos = bBulletPosTop.transform.position;

                bBulletLocalRotation = bBulletPosTop.rotation;
            }

            if (i > 2)
            {
                //Set deafault position and location
                bBulletLocalPos = bBulletPosBtm.transform.position;

                bBulletLocalRotation = bBulletPosBtm.rotation;
            }

            if (i == 0 || i == 3)
            {
                beBullet.eBulletPattern = 1;
            }

            if (i == 1 || i == 4)
            {
                beBullet.eBulletPattern = 2;
            }

            if (i == 2 || i == 5)
            {
                beBullet.eBulletPattern = 3;
            }

            Instantiate(bBullet, bBulletLocalPos, bBulletLocalRotation);
            yield return new WaitForSeconds(Random.Range (0.2f, 0.8f));
            //yield return null;            
        }
    }
}
